﻿using MongoDB.Driver;

namespace minimalApiMongo.Services
{
    public class MongoDbService
    {
        /// <summary>
        /// Armazenar a configuração da aplicação
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Armazena a referencia ao MongoDb
        /// </summary>
        private readonly IMongoDatabase _database;


        /// <summary>
        /// Contém a configuração nescessaria para acesso ao MongoDb
        /// </summary>
        /// <param name="configuration">Objeto constendo toda a configuração da aplicação</param>
        public MongoDbService(IConfiguration configuration)
        {
            //Atribui a config recebida em _configuration
            _configuration = configuration;


            //Acessa a string de conexão
            var connectionString = _configuration.GetConnectionString("DbConnection");

            //Transforma a string obtida em MongoURl
            var mongoUrl = MongoUrl.Create(connectionString);

            //Cria um client 
            var mongoClient = new MongoClient(mongoUrl);

            //Obtém a referencia ao MongoDb
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// Propriedade para acessar o bd => retorna os dados em _database
        /// </summary>
        public IMongoDatabase GetDatabase => _database;
    }
}
