## Notes

Test-drive all about [NATS Docker](https://docs.nats.io/running-a-nats-service/nats_docker)

Running NATS in Docker:

`nats-cluster.yaml` - in a cluster
`nats-single.yaml`  - as a single instance

`docker-compose -f .\nats-cluster.yaml up`
`docker-compose -f .\nats-single.yaml up`