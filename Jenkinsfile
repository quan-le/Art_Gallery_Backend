pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = "1"
        BUILD_CONFIG = "Release"
        API_IMAGE = "art-gallery-api:latest"
        API_CONTAINER = "artgallery-api"
        //API_PORT = "5000"
        API_PORT = "80"
        SONAR_TOKEN = credentials('Sonar_token')
        SONAR_ORG = "quan-le"
        SONAR_PROJECT_kEY = "quan-le_Art_Gallery_Backend"
        NVD_API_KEY = credentials('NVD_API_KEY')
        OCTOPUS_URL = 'https://quanle.octopus.app'
        OCTOPUS_API_KEY = credentials('OCTOPUS_API_KEY')
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
                sh "docker run -d -p ${API_PORT}:8080 --name ${API_CONTAINER} --add-host host.docker.internal:host-gateway ${API_IMAGE}"

                // Run newman test
                echo "Running Postman collection tests..."
                //sh "docker exec ${API_CONTAINER} npx newman run /app/5.2HD_ArtGallery.postman_collection.json --reporters cli -k --verbose"
                sh "docker exec ${API_CONTAINER} npx newman run /app/5.2HD_ArtGallery.postman_collection.json --reporters cli -k --verbose"

                // Optional: stop and remove the container after tests
                /*
                echo "Stopping and removing temporary container"
                sh "docker rm -f artgallery-api"
                */
            }
        }
        
        // ======================
        stage('Code Quality Analysis') {
            steps {
                script{
                    echo "Code Quality stage"
                    withSonarQubeEnv('SonarCloud') {
                        dir('Art_Gallery') {
                            // Start analysis
                            sh """
                                dotnet tool install --global dotnet-sonarscanner || true
                                export PATH="$PATH:/root/.dotnet/tools"
                                dotnet sonarscanner begin \
                                    /k:"${SONAR_PROJECT_KEY}" \
                                    /o:"${SONAR_ORG}" \
                                    /d:sonar.login="${SONAR_TOKEN}" \
                                    /d:sonar.host.url="https://sonarcloud.io"
                                
                                dotnet build --configuration ${BUILD_CONFIG}
                                
                                dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"
                            """
                        }
                    }
                }     
            }
        }

        // ======================
        stage('Security') {
            steps {
                echo "Security Analysis stage"
                sh 'mkdir -p dependency-check-report'

                dependencyCheck additionalArguments: '''
                    --scan Art_Gallery
                    --format "ALL"
                    --out dependency-check-report
                    --nvdApiKey a7ed2a3f-41ff-45eb-a682-3cda27de56cb
                ''', odcInstallation: 'DependencyCheck'
            }
            post {
                always {
                    echo "Publishing Dependency-Check Reports:"
                    dependencyCheckPublisher pattern: '**/dependency-check-report/*.xml'
                    echo "Dependency-Check HTML Report Output:"
                    sh '''
                        if [ -f dependency-check-report/dependency-check-report.html ]; then
                            cat dependency-check-report/dependency-check-report.html
                        else
                            echo "No HTML report found."
                        fi
                    '''
                }
                failure {
                    echo "Security vulnerabilities detected. Review report in Jenkins or HTML output."
                }
            }
        }

        // ======================
        stage('Deploy') {
            steps {
                echo "Deploy stage"
                script {
                    echo "Deploying API to staging environment using Docker Compose"
                    dir('Art_Gallery') {
                        sh 'docker rm -f artgallery-api-deploy 2>/dev/null || true'
                        sh 'docker-compose -f docker-compose.staging.yml down -v || true'

                        sh 'docker-compose -f docker-compose.staging.yml up -d'
                        
                        sh 'docker ps'
                    }
                }
            }
        }
        /*
        // ======================
        stage('Release') {
            steps {
                echo "Release stage"
                echo "Pushing Docker Image to Docker HUb for release"
                script {
                    sh """
                        docker tag ${API_IMAGE} quanle/art-gallery-api:${BUILD_NUMBER}
                        docker tag ${API_IMAGE} quanle/art-gallery-api:latest
                        docker push quanle/art-gallery-api:${BUILD_NUMBER}
                        docker push quanle/art-gallery-api:latest
                    """
                    echo "Triggering Octopus Deplpoy release"
                    def version = "${BUILD_NUMBER}"
                    def projectName = "Art Gallery API"
                    sh """
                    curl -X POST ${OCTOPUS_URL}/api/releases \
                        -H "X-Octopus-ApiKey: ${OCTOPUS_API_KEY}" \
                        -H "Content-Type: application/json" \
                        -d '{
                            "Version": "${version}",
                            "ReleaseNotes": "Automated release from Jenkins build #${BUILD_NUMBER}"
                        }'
                    """
                    
                    echo "Octopus Release triggered successfully."
                }
            }
        }

        // ======================
        stage('Monitoring') {
            steps {
                echo "Monitoring stage"
            }
        }
        */
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
