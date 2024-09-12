# Aplicação Mensageria - .NET e Kafka
As aplicações utilizam Kafka para comunicação assíncrona, com a Aplicação 1 aguardando a resposta da Aplicação 2 para garantir que o processamento da ordem foi bem-sucedido antes de responder ao cliente HTTP.

### Kafka
![](https://raw.githubusercontent.com/brunol-pereira/queue_application/main/Imagens/Kafka.PNG)

- Docker Compose -> kafka/docker-compose.yml

### Produtor iniciado
![](https://raw.githubusercontent.com/brunol-pereira/queue_application/main/Imagens/ProdutorIniciado.PNG)

### Consumidor iniciado
![](https://raw.githubusercontent.com/brunol-pereira/queue_application/main/Imagens/ConsumidorIniciado.png)

### Envio da mensagem
![](https://raw.githubusercontent.com/brunol-pereira/queue_application/main/Imagens/EnvioMensagem.PNG)

### Produtor - mensagem enviada
![](https://raw.githubusercontent.com/brunol-pereira/queue_application/main/Imagens/ProdutorMensagem.PNG)

### Consumidor - mensagem recebida e confirmação enviada
![](https://raw.githubusercontent.com/brunol-pereira/queue_application/main/Imagens/MensagemRecebidaEnviada.png)

### Resposta ao Cliente
![](https://raw.githubusercontent.com/brunol-pereira/queue_application/main/Imagens/RespostaCliente.PNG)

