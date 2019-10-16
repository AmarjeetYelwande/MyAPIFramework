def imageRegistry = 'amarjeet/myrepository'

def imageName = "${imageRegistry}/api-tests"

def imageTag = null

def buildImage = null

def version = 'latest'

node {
    stage('Checkout') {
        deleteDir()
        checkout scm
		echo "Finished checking out code. Now entering Build stage..."
    }
    stage('Build') {
        imageTag = "${imageName}:${version}"
        buildImage = docker.build(imageTag, "--no-cache .")
		echo "Finished building image. Now entering Publish stage..."
    }
    stage('Publish') {       

        docker.withRegistry('https://registry.hub.docker.com', 'dockerhub') {
            buildImage.push(version)
        }
    }
}
