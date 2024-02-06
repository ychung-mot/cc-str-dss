terraform {
  required_version = ">= 0.15.3"

  backend "kubernetes" {
    namespace     = "b07b69-dev"
    secret_suffix = "state" # pragma: allowlist secret
    config_path   = "~/.kube/config"
  }
}

provider "kubernetes" {
  config_path = "~/.kube/config"
}
