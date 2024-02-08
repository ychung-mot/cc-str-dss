oc login
oc project b07b69-dev / test / prod

helm install postgresql-dev bitnami/postgresql -f values-dev.yaml

helm install postgresql-test bitnami/postgresql -f values-test.yaml

helm install postgresql-uat bitnami/postgresql -f values-uat.yaml

helm install postgresql-prod bitnami/postgresql -f values-prod.yaml

