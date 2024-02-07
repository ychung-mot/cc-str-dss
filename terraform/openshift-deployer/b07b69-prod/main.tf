module "fdf5df_deployer" {
  source  = "bcgov/openshift/deployer"
  version = "0.11.0"

  name                  = "oc-deployer"
  namespace             = "b07b69-prod"
  privileged_namespaces = ["b07b69-prod"]
}
