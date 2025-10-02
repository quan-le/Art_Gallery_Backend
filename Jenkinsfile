pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = "1"
        BUILD_CONFIG = "Release"
        API_IMAGE = "art-gallery-api:latest"
        API_CONTAINER = "artgallery-api"
        API_PORT = "5000"
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm  
            }
        }
        // =====================
        stage('Build') {
            steps {
                script {
                    echo "Building Docker image for the API"
                    sh "docker build -t ${API_IMAGE} ."

                    echo "Publishing dotnet artifacts (optional)"
                    sh "dotnet restore"
                    sh "dotnet build --configuration ${BUILD_CONFIG}"
                    sh "dotnet publish -c ${BUILD_CONFIG} -o publish"
                    
                    // Archive artifacts in Jenkins
                    archiveArtifacts artifacts: 'publish/**', fingerprint: true
                }
            }
        }

        // ======================
        stage('Test') {
            steps {
                echo "Running API in temporary Docker container"
                    sh "docker run -d -p ${API_PORT}:8080 --name ${API_CONTAINER} ${API_IMAGE}"

                    echo "Running integration tests against API"
                    // Example: health check or Postman/Newman
                    sh "curl -f http://localhost:${API_PORT}/health || exit 1"

                    echo "Stop and remove temporary container"
                    sh "docker rm -f ${API_CONTAINER}"
            }
        }

        // ======================
        stage('Code Quality') {
            steps {
                echo "Code Quality stage"
                
            }
        }

        // ======================
        stage('Security') {
            steps {
                echo "Security stage"
                
            }
        }

        // ======================
        stage('Deploy') {
            steps {
                echo "Deploy stage"
            }
        }

        // ======================
        stage('Release') {
            steps {
                echo "Release stage"
            }
        }

        // ======================
        stage('Monitoring') {
            steps {
                echo "Monitoring stage"
            }
        }
    }

    post {
        success {
            echo "Pipeline completed successfully!"
        }
        failure {
            echo "Pipeline failed. Check the logs."
        }
        always {
            echo "Cleaning up workspace..."
            cleanWs()
        }
    }
}
