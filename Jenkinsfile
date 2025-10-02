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
                    sh "docker build -t ${API_IMAGE} -f Art_Gallery/Dockerfile Art_Gallery"
                    /*
                    echo "Publishing dotnet artifacts"
                    dir('Art_Gallery') {
                        sh 'dotnet restore'
                        sh "dotnet build --configuration ${BUILD_CONFIG}"
                        sh "dotnet publish -c ${BUILD_CONFIG} -o publish"
                    }

                    // Archive artifacts
                    archiveArtifacts artifacts: 'Art_Gallery/publish/**', fingerprint: true
                    
                    */
                }
            }
        }

        // ======================
        stage('Test') {
            steps {
                echo "Running API in temporary Docker container"

                // Remove any previous container
                sh "docker rm -f artgallery-api 2>/dev/null || true"

                // Run container using the baked-in appsettings.json
                sh "docker run -d -p ${API_PORT}:8080 --name ${API_CONTAINER} ${API_IMAGE}"
                
                // Wait for the API to initialize
                echo "Waiting 30 seconds for API to initialize..."
                sh "sleep 30"

                echo "You can now check the API manually at http://localhost:5000"

                // Run a basic health check
                echo "Running integration tests against API"
                sh "curl -f http://localhost:5000/health || exit 1"

                // Optional: stop and remove the container after tests
                /*
                echo "Stopping and removing temporary container"
                sh "docker rm -f artgallery-api"
                */
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
            script{
                echo "Cleaning up workspace..."
                cleanWs()
            }
        }
    }
}
