# OnlineStoreBackend

## Stack: 
- ElasticSearch (7.17.3)
- Rest API (.Net 6)
- ~~PostgreSQL~~ (used ES for categories too)
- Nest (ES high level client)
- Xunit 
- Serilog
- Swagger

## Goals:
- [x] CRUD: product
- [x] CRUD: category
- [x] Use elastic to search through all products
- [x] Create search endpoint to search through products & categories
- [ ] Background job for all products reindex in ES
- [ ] Add localization (EN by default)
- [x] Unit integration tests _(but not products)_
- [x] Add migrator for index creation

## May be sometime (think about realization)
Facets, search by multiple facets  
Product boostings  
Product stocks in shops  
Authentication  

# Install & Run 
1. Install docker
2. Pull & Run elastic search image via command   
  `docker run -d --name elasticsearch --net elastic -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:7.17.3`
3. OPTIONAL: pull & run kibana image via command
  `docker run --name kibana --net elastic -p 5601:5601 docker.elastic.co/kibana/kibana:7.17.3`
4. Restore nuget packages
5. Run _OnlineStoreBackend.Migration_ project
6. Run _OnlineStoreBackend_ project
7. Swagger is available at http://localhost:5000/swagger endpoint

# Known issues:
- Awaiting `Task.Delay(1 second)` used because create document operation takes time to index new documents 
