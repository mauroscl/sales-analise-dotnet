# sales-analise-dotnet
Implementa estatísticas sobre dados de venda disponibilizados em um arquivo CSV.
# Problema
Criar um sistema de análise de dados de venda que irá importar lotes de arquivos e produzir
um relatório baseado em informações presentes no mesmo.
Existem 3 tipos de dados dentro dos arquivos e eles podem ser distinguidos pelo seu
identificador que estará presente na primeira coluna de cada linha, onde o separador de
colunas é o caractere “ç”.

#### Dados do vendedor
Os dados do vendedor possuem o identificador 001 e seguem o seguinte formato:

001çCPFçNameçSalary

####Dados do cliente
Os dados do cliente possuem o identificador 002 e seguem o seguinte formato:

002çCNPJçNameçBusiness Area

#### Dados de venda
Os dados de venda possuem o identificador 003 e seguem o seguinte formato:

003çSale IDç[Item ID-Item Quantity-Item Price]çSalesman name

#### Exemplo de conteúdo total do arquivo:

```
001ç1234567891234çPedroç50000
001ç3245678865434çPauloç40000.99
002ç2345675434544345çJose da SilvaçRural
002ç2345675433444345çEduardo PereiraçRural
003ç10ç[1-10-100,2-30-2.50,3-40-3.10]çPedro
003ç08ç[1-34-10,2-33-1.50,3-40-0.10]çPaulo
```

O sistema deverá ler continuamente todos os arquivos dentro do diretório padrão
HOMEPATH/data/in e colocar o arquivo de saída em HOMEPATH/data/out.
No arquivo de saída o sistema deverá possuir os seguintes dados:

- Quantidade de clientes no arquivo de entrada

- Quantidade de vendedores no arquivo de entrada

- ID da venda mais cara

- O pior vendedor


# Solução

Não ficou tão claro se o arquivo de saída deve ser processado de forma cumulativa ou independentemente.
Eu escolhi processar cada arquivo independentemente. Então quando a aplicação receive um arquivo no diretório de entrada será gerado um arquivo de saída com o resultado do processamento referente a este arquivo.

O arquivo de saída tem o sufixo "done" no seu nome. Se o arquivo de entrada for "vendas1.dat" então o arquivo de saída será "vendas1.done.dat"  

## Input Example:
```sh
001ç1234567891234çDiegoç50000 
001ç3245678865434çRenatoç40000.99
002ç2345675434544345çJose da SilvaçRural 
002ç2345675433444345çEduardo PereiraçRural
003ç10ç[1-10-100,2-30-2.50,3-40-3.10]çDiego
003ç08ç[1-34-10,2-33-1.50,3-40-0.10]çRenato
```

## Output
```sh
amountSalesman=2
amountCustomer=2
worstSellers=[Renato]
mostExpensiveSales=[10]
```
Piores vendedores e vendas mais caras serão separados por vírgula quando houver empate.

## Tecnologias utilizadas

A solução foi implementada utilizando as seguintes tecnologias:
- Dot Net Core
- Apache Kafka
- File Helpers (https://www.filehelpers.net/)
- MsTest and Moq

## Arquitetura da Solução
Foram implementadas duas Console Application para resolver o problema:
- SalesProcessor: responsável por detectar quando novos no diretório de entrada e salvar o resultado do processamento
- SalesAnalyzer: responsável por processar o conteúdo das vendas e enviar a resposta para o SalesProcessor

Foi utilizado o ApacheKafka como mecanismo de mensageria para fins de desacoplamento e escabilidade.
Com isso podemos ter N instâncias de SalesAnalyzer (cada uma apontando para um diretório diferente) e também N instâncias de SalesProcessor rodando ao mesmo tempo, conforme a necessidade.

### Troca de mensagens
Foram criados dois tópicos: 
- `sales-analysis-input`: contem as mensagens para processamento. Enviado pelo Processor e consumido pelo Analyzer  
- `sales-analysis-output<sufixo>`: contém o resultado do processamento. Enviado pelo Analyzer e consumido pelo  Processor

Existe um tópico de resultados (sales-analysis-output)  para cada instância de SalesProcessor que estiver rodando. Por isso o nome deste tópico contém um sufixo. O sufixo é o id da aplicação SalesProcessor informado como argumento, quando a mesma é executada.

Houve a necessidade da criação deste sufixo porque a aplicação SalesAnalyzer desconhece quem mandou as mensagens. Se as mesmas fossem respondidas em um único tópico, quando houvesse mais de uma instância de SalesProcessor rodando, as mesmas receberiam o resultado de todo o processamento e não apenas referentes às mensagens que ela enviou.

Então, a cada mensagem que SalesProcessor envia, contém um cabeçalho `CTM_SALES_ANALYSIS_OUTPUT_TOPIC` contendo o nome do tópico que o SalesAnalyzer deve enviar o resultado.

## Arquitetura das aplicações
Foi utilizada uma arquitetura hexagonal na solução.
Principais projetos e suas responsabilidades:

#### Application
Contém a parte de negócio, é independente de tecnologia. Por exemplo em SalesAnalyzer, atualmente utilizamos o Kafka como entrada e saída para o processamento. Mas poderíamos utlizar rest para entrada e arquivo para saída, sem precisar alterar Application

Pacote | Descrição
--- | ---
domain| Contém as entidades do sistema. Pode conter também objetos de valor e serviços de domínio
driver ports| são as portas de entrada para a aplicação, ou seja, para executar os casos de uso. São as interfaces que definem os contratos que serão chamdos pelos Primary Adapters
driven ports| são portas que a aplicação utilizada para acessar o mundo externo. São as interfaces que definem os contratos implementados pelos Secondary Adapters
usecases|Coordena a lógica de implementação do caso de uso. Utiliza os objetos do domínio e as `driven ports` para realizar este trabalho 

#### Adapters
Implenta as portas definidas na aplicação

Pacote | Descrição
--- | ---
primary|SalesProcessor, por exemplo, tem como porta de entrada para inicia do processamento o File System e depois o Kafka para a persistência dos resultados. Mas poderia ter um adaptor para requisições HTTP, que neste caso poderia ser um API Controller, por exemplo.
secondary|SalesProcessor, por exemplo, implementa adapters para o sistema de arquvivo e para o sistema de mensageria do Kafka

## Como rodar
Podemos rodar a aplicação de duas formas: manualmente e via docker-compose

### Manualmente

#### Rodar Kafka
executar o comando `docker-compose -f docker-compose-kafka.yml up` a partir da raiz do código fonte
#### Rodar Sales Analyzer
É necessário passar como variável de ambiente o endereço do servidor do Kafka.
Formato do comando:
`
dotnet run -p SalesAnalyzer.ConsoleApp --KAFKA_SERVER=localhost:9092
´
E então teremos como saída no console
```
Running Sales Analyzer...
```
Podem ser rodadas n instâncias desta aplicação. Mas para a escabilidade funcionar o número de partições do tópico `sales-analysis-input` devem ser igual ao número de consumidores.

Para aumentar o número de partições do tópico é necessário acessar o container do kafka para executar um script.
Para isso é necessário obter o id do container. Pode ser usado o seguinte comando:
```
docker ps -a | grep kakfa
```
que vai retornar 
```
961a79624fc3        wurstmeister/kafka       "start-kafka.sh"         4 minutes ago       Up 3 minutes        0.0.0.0:9092->9092/tcp                               sales-analise-dotnet_kafka_1
```
Com o id do container sendo `961a79624fc3` devemos acessar o container com o comando
```
docker exec -it 961a79624fc3 /bin/bash
```
E então executar o comando para aumentar o número de partições. No exemplo, o número de partições é aumentado para 2.
```
/opt/kafka/bin/kafka-topics.sh --zookeeper zookeeper:2181 --alter --topic  sales-analysis-input --partitions 2
```
Após esse comando podemos descrever o tópico para saber se as mudanças surtiram efeito
```
/opt/kafka/bin/kafka-topics.sh --describe --topic sales-analysis-input --zookeeper zookeeper:2181
```
E então teremos o seguinte resultado
```
Topic:sales-analysis-input      PartitionCount:2        ReplicationFactor:1     Configs:
        Topic: sales-analysis-input     Partition: 0    Leader: 1001    Replicas: 1001  Isr: 1001
        Topic: sales-analysis-input     Partition: 1    Leader: 1001    Replicas: 1001  Isr: 1001
```

#### Rodar Sales Processor

 É necessário passar parâmetros na seguinte ordem:
- Identificador da aplicação
- Kafka Server
- Diretório de entrada
- Diretório de saída
Formato do comando:
`dotnet run -p SalesProcessor.ConsoleApp app01 localhost:9092 data/in data/out`
Então teremos como saída no console
``` 
Running Sales Processor...
ENVIRONMENT...
APPLICATION IDENTIFIER: RDpcVXNlcnNCMEY5QlE=
KAFKA SERVER: localhost:9092
INPUT PATH: data/in
OUTPUT PATH: data/out
Watching folder D:\Users\mauro\dev\sales-analise-dotnet\data\in
```
Pode ser executadas n instâncias de SalesProcessor, uma em cada pasta

Na pasta arquivos-exemplo, existem alguns arquivos que  podem ser usados como input para teste

### docker-compose

Antes de executar o docker-compose é necessário obter o ip do host do docker. Houve uma tentativa de obter automaticamente, mas não funcionou.
Para obter o IP deve ser executado o comando 
```
ip -4 addr show docker0 | grep -Po 'inet \K[\d.]+'
```
Após obter o IP editar o arquivo `docker-compose.yml` e substituir o IP na variável de ambiente `KAFKA_ADVERTISED_HOST_NAME`

Executar o comando `docker-compose up`. Este comando vai iniciar uma instância de cada serviço.

O serviço sales-processor não deve ser escalado via docker-compose, pois duas instâncias rodariam escutando o mesmo diretório.

Para escalar o serviço sales-analyzer, é necessário primeiro aumentar o número de partições do tópico `sales-analysis-input`. Para fazer isso, devem ser seguidas as mesmas instruções da seção `Rodar Sales Analyzer`.

Em seguida deve ser executado o comando `docker-compose scale sales-analyzer=2`. No exemplo, estamos aumentando para duas instâncias. Pode ser informado o número de instâncias que for necessário.

Importante reforçar que o número de instâncias do serviço deve ser igual ao número de partições. Se o número de instâncias for maior, alguns consumidores ficarão em idle.

## Exemplo de execução
Ao colocar um arquivo no arquivo no diretório de entrada temos a seguinte saída em SalesProcessor
```
File: data/in\arquivo1.dat Created
Content of file arquivo1.dat sent to kafka
File arquivo1.dat processed
```
Em SalesAnalyzer temos a seguinte saída:
```
-------------------------------------------
Message processed
Custom Headers:
CTM_FILE_NAME: arquivo1.dat
CTM_SALES_ANALYSIS_OUTPUT_TOPIC: sales-analysis-output-app01
-------------------------------------------
```

