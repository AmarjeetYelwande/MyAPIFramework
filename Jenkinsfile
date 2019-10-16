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

        docker.withRegistry("https://${imageRegistry}", 'MY_ACCOUNT') {
            buildImage.push(version)
        }
    }
    stage('Deploy') {
     build job: 'temp-docker-deploy/my-api-tests', parameters: [string(name: 'IMAGE_TAG', value: version)]
    }
 
    stage('Run-Integration-Tests') {
         build job: 'my-api-testing', parameters: [string(name: 'BRANCH', value: 'master')], wait: false
    }
}
