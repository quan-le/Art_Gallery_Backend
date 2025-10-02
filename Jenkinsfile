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

                // Remove old container if exists
                sh """
                docker rm -f ${API_CONTAINER} 2>/dev/null || true
                """

                // Run container with appsettings.json directly from repo
                sh """
                PORT=${API_PORT}
                CONTAINER=${API_CONTAINER}
                IMAGE=${API_IMAGE}

                docker run -d -p $PORT:8080 \
                    --name $CONTAINER \
                    -v ${WORKSPACE}/appsettings.json:/app/appsettings.json:ro \
                    $IMAGE
                """

                echo "Waiting 30 seconds for API to initialize..."
                echo "You can now check the API manually at http://localhost:${API_PORT}"
                sh "sleep 30"

                echo "Running integration test for /health endpoint"
                sh "curl -f http://localhost:${API_PORT}/scalar || exit 1"

                /*
                
                Stop container after tests
                sh "docker rm -f ${API_CONTAINER}"
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
