# OnlineStoreBackend
online store backend (REST API + Elastic)

## Stack: 
- ElasticSearch (7.17.3)
- Rest API
- PostgreSQL
- Xunit

# Goals:
- [x] CRUD: product
- [x] CRUD: category
- [ ] ~~Configure automapper~~ no need
- [ ] Use elastic to search through all products
- [ ] Create search endpoint to search through products&categories
- [ ] Add localization (EN by default)
- [ ] Unit integration tests

## May be sometime (think about realization)
Facets, search by multiple facets    
Product stocks in shops  

### Entities
Category:  
- id
- name
- isActive
- path
- parent (empty = root cat)

Product:
- id
- name
- code
- isActive
- updated_at
- path

Store:
- id
- address
  - long
  - lat