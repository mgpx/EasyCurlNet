# EasyCurlNet
Este projeto ajuda a realizar requisições HTTP, em sistemas Windows aonde o conjunto de criptografia não é mais suportado.
Ao invés de realizar a requisição por meio de componentes nativos do .net framework, este usa o curl + openssl.

Este projeto foi baseado no [CurlThin](https://github.com/stil/CurlThin), com algumas mudanças.

Criação da classe `EasyHttp` para facilitar o uso.

Exemplo:

     using (var http = new EasyHttp("https://endereco.com/"))
     {
         string jsonBody = "{\"key\": \"exemplo\", \"id\": \"10\"}";
         http.AddHeader("Content-Type", "application/json");
         var responseString = http.Execute("api/v1/getOrders", Method.POST, jsonBody);
     }
 
Atualmente a aplicação conta apenas com recursos básicos, adicionar headers e métodos GET/POST/PUT/DELETE.

