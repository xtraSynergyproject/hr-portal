pipeline {
    agent any 
    environment {
    DOCKERHUB_CREDENTIALS = credentials('privateregistrycredentials')
    }
    stages { 
        stage('Build docker app image') {
                            steps {
                dir('src') {
                                script   {  
                                    sh 'docker build -t synergysolution/cms_synergy_web:latest -f Dockerfile-Web .'
                            }
                            }
                        }
        }
        stage('Build docker api image') {
                            steps {
                dir('src') {
                                script   {  
                                    sh 'docker build -t synergysolution/cms_synergy_api:latest -f Dockerfile-Api .'
                            }
                            }
                        }
        }
        stage('login to dockerhub') {
            steps{
                sh 'echo $DOCKERHUB_CREDENTIALS_PSW | docker login -u $DOCKERHUB_CREDENTIALS_USR --password-stdin'
            }
        }
        stage('push app image') {
            steps{
                sh 'docker push synergysolution/cms_synergy_web:latest'
            }
        }
        stage('push api image') {
            steps{
                sh 'docker push synergysolution/cms_synergy_api:latest'
            }
        }
}
post {
        always {
            sh 'docker logout'
        }
    }
}