pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = "1"
        BUILD_CONFIG = "Release"
        API_IMAGE = "art-gallery-api:latest"
        API_CONTAINER = "artgallery-api"
        //API_PORT = "5000"
        API_PORT = "80"
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
                sh "docker run -d -p ${API_PORT}:8080 --name ${API_CONTAINER} --add-host "host.docker.internal:host-gateway" ${API_IMAGE}"

                // Run newman test
                echo "Running Postman collection tests..."
                sh "docker exec ${API_CONTAINER} npx newman run /app/5.2HD_ArtGallery.postman_collection.json --reporters cli"

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
