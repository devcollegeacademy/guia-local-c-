# Use a imagem oficial do Ubuntu como base
FROM ubuntu:latest

# Defina variáveis de ambiente para a instalação do Oracle XE 11g
ENV ORACLE_HOME=/u01/app/oracle/product/11.2.0/xe
ENV ORACLE_SID=XE
ENV PATH=$ORACLE_HOME/bin:$PATH

# Configurações para instalação silenciosa
ENV DEBIAN_FRONTEND noninteractive

# Atualize os pacotes e instale as dependências
RUN apt-get update && \
    apt-get install -y libaio1 net-tools && \
    apt-get clean

# Copie os arquivos de instalação do Oracle XE 11g para o diretório temporário
COPY oracle-xe-11.2.0-1.0.x86_64.rpm /tmp/

# Executa a instalação silenciosa do Oracle XE 11g
RUN dpkg -i /tmp/oracle-xe-11.2.0-1.0.x86_64.rpm && \
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

# Exponha a porta padrão do Oracle XE
EXPOSE 1521

# Copie o arquivo de inicialização do Oracle
COPY init.ora /etc/init.d/

# Defina permissões e inicie o Oracle XE
RUN chmod 755 /etc/init.d/init.ora && \
    ln -s /etc/init.d/init.ora /etc/rc2.d/S01init.ora && \
    /etc/init.d/init.ora start

# Comando padrão para iniciar o Oracle XE quando o contêiner é iniciado
CMD ["bash"]
