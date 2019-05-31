#!/bin/bash


#判断是否是root用户
user=$(env | grep USER | cut -d "=" -f 2)
[ "$user" != "root" ] && echo "only root can run it" && exit -1

#设置免密登陆
IP=`hostname -I`
echo "$IP master" >> /etc/hosts

rm -rf ~/.ssh/{known_hosts,id_rsa*}
ssh-keygen -t rsa -n '' -f ~/.ssh/id_rsa
cat ~/.ssh/id_rsa.pub >> ~/.ssh/authorized_keys
chmod 600 ~/.ssh/authorized_keys


HADOOP_HOME="/usr/local/hadoop"
JAVA_HOME="/usr/local/jdk"
HBASE_HOME="/usr/local/hbase"
HIVE_HOME="/usr/local/hive"
SPARK_HOME="/usr/local/spark"
ZK_HOME="/usr/local/zk"

#下载文件
[ ! -f "./hadoop-2.6.0-cdh5.7.0.tar.gz" ] && wget http://archive.cloudera.com/cdh5/cdh/5/hadoop-2.6.0-cdh5.7.0.tar.gz 
[ ! -f "./hbase-1.2.0-cdh5.7.0.tar.gz" ] && wget http://archive.cloudera.com/cdh5/cdh/5/hbase-1.2.0-cdh5.7.0.tar.gz
[ ! -f "./hive-1.1.0-cdh5.7.0.tar.gz" ] && wget http://archive.cloudera.com/cdh5/cdh/5/hive-1.1.0-cdh5.7.0.tar.gz
[ ! -f "./zookeeper-3.4.5-cdh5.7.0.tar.gz" ] && wget http://archive.cloudera.com/cdh5/cdh/5/zookeeper-3.4.5-cdh5.7.0.tar.gz
[ ! -f "./spark-1.6.0-cdh5.7.0.tar.gz" ] && wget http://archive.cloudera.com/cdh5/cdh/5/spark-1.6.0-cdh5.7.0.tar.gz
[ ! -f "./OpenJDK8U-jdk_x64_linux_hotspot_8u202b08.tar.gz" ] && wget https://github.com/AdoptOpenJDK/openjdk8-binaries/releases/download/jdk8u202-b08/OpenJDK8U-jdk_x64_linux_hotspot_8u202b08.tar.gz

#清除本地文件
rm -rf /usr/local/hadoop /usr/local/hbase /usr/local/hive /usr/local/zk /usr/local/spark

#解压文件
tar -zvxf hadoop-2.6.0-cdh5.7.0.tar.gz  && mv hadoop-2.6.0-cdh5.7.0 /usr/local/hadoop
tar -zvxf hbase-1.2.0-cdh5.7.0.tar.gz && mv hbase-1.2.0-cdh5.7.0 /usr/local/hbase
tar -zvxf hive-1.1.0-cdh5.7.0.tar.gz  && mv hive-1.1.0-cdh5.7.0 /usr/local/hive
tar -zvxf zookeeper-3.4.5-cdh5.7.0.tar.gz &&  mv zookeeper-3.4.5-cdh5.7.0 /usr/local/zk
tar -zvxf spark-1.6.0-cdh5.7.0.tar.gz && mv spark-1.6.0-cdh5.7.0 /usr/local/spark
tar -zvxf OpenJDK8U-jdk_x64_linux_hotspot_8u202b08.tar.gz && mv jdk8u202-b08 /usr/local/jdk

#配置环境变量
cat >> /etc/profile <<EOT

export HADOOP_HOME=${HADOOP_HOME}
export JAVA_HOME=${JAVA_HOME}
export HBASE_HOME=${HBASE_HOME}
export HIVE_HOME=${HIVE_HOME}
export SPARK_HOME=${SPARK_HOME}
export ZK_HOME=${ZK_HOME}

export PATH=\$PATH:\$HADOOP_HOME/bin:\$HADOOP_HOME/sbin:\$JAVA_HOME/bin:\$HBASE_HOME/bin:\$HIVE_HOME/bin:\$SPARK_HOME/bin:\$ZK_HOME/bin:\$ZK_HOME/sbin:\$ZK_HOME/sbin

EOT



#Hadoop文件配置
#hadoop-env.sh
sed -i  '/${JAVA_HOME}/s/${JAVA_HOME}/\/usr\/local\/jdk/g'  $HADOOP_HOME/etc/hadoop/hadoop-env.sh

#core-site.xml
A=$(sed -n '$=' $HADOOP_HOME/etc/hadoop/core-site.xml)
B=$(sed -n '$=' $HADOOP_HOME/etc/hadoop/hdfs-site.xml)
sed -i $(($A-2+1)),${A}d $HADOOP_HOME/etc/hadoop/core-site.xml
sed -i $(($A-2+1)),${B}d $HADOOP_HOME/etc/hadoop/hdfs-site.xml


cat >> $HADOOP_HOME/etc/hadoop/core-site.xml <<EOT
<configuration>
     <property>
         <name>fs.defaultFS</name>
         <value>hdfs://master:9000</value>
     </property>
	 <property>
        <name>hadoop.tmp.dir</name>
        <value>file:/usr/local/hadoop/data/tmp</value>
        <description>Abase for other temporary directories.</description>
    </property>
</configuration>
EOT

#hdfs-site.xml
cat >> $HADOOP_HOME/etc/hadoop/hdfs-site.xml <<EOT
<configuration>
      <property>
         <name>dfs.replication</name>
         <value>1</value>
      </property>
	  <property>
         <name>dfs.permissions.enabled</name>
         <value>false</value>
      </property>
</configuration>
EOT

#hbase




# export JAVA_HOME=/usr/java/jdk1.6.0/
sed -i  '/# export JAVA_HOME=/s/# export JAVA_HOME=\/usr\/java\/jdk1.6.0\//export JAVA_HOME=\/usr\/local\/jdk/g'  $HBASE_HOME/conf/hbase-env.sh

B=$(sed -n '$=' $HBASE_HOME/conf/hbase-site.xml)
echo "master" >> $HBASE_HOME/conf/regionservers
cat >> $HBASE_HOME/conf/hbase-site.xml <<EOT
  <property>
    <name>hbase.rootdir</name>
    <value>hdfs://master:9000/hbase</value>
  </property>
  <property>
    <name>hbase.zookeeper.property.dataDir</name>
    <value>/usr/local/hbase/data/zookeeper</value>
  </property>
  <property>
    <name>hbase.cluster.distributed</name>
    <value>true</value>
  </property>
EOT

#启动
$HADOOP_HOME/bin/hadoop namenode -format
$HADOOP_HOME/sbin/start-dfs.sh
$HBASE_HOME/bin/hbase-daemon.sh start zookeeper
$HBASE_HOME/bin/hbase-daemon.sh start master
$HBASE_HOME/bin/hbase-daemon.sh start regionserver












