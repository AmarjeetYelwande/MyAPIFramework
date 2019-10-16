def imageRegistry = 'amarjeet/myrepository'

def imageName = "${imageRegistry}/api-tests"

def imageTag = null

def buildImage = null

def version = 'latest'

node {
    stage('Checkout') {
        deleteDir()
        checkout scm
    }
    stage('Build') {
        imageTag = "${imageName}:${version}"
        buildImage = docker.build(imageTag, "--no-cache .")
    }
    stage('Publish') {       

        docker.withRegistry('https://registry.hub.docker.com', 'dockerhub') {
            buildImage.push(version)
        }
    }
}
