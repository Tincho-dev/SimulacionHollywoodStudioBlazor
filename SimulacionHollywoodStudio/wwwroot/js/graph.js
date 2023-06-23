window.plotFunction = (inputX) => {
    // Crear un array para tus datos
    let data = [];

    for (let i = 0; i <= 500; i += 50) {
        data.push({ x: i, y: 161 * i });
    }

    inputX = Math.round(inputX);
    // Calcular el valor y correspondiente para inputX
    let inputY = 161 * inputX;

    // Obtener el elemento canvas del DOM
    let ctx = document.getElementById('myChart').getContext('2d');

    // Destruir el gráfico existente si existe
    if (myChart) {
        myChart.destroy();
    }

    // Crear el gráfico
    myChart = new Chart(ctx, {
        type: 'line',
        data: {
            datasets: [
                {
                    label: 'y = 161*x',
                    data: data,
                    borderColor: 'rgba(255,99,132,1)',
                    borderWidth: 1
                },
                {
                    label: `Punto (${inputX}, ${inputY})`,
                    data: [{ x: inputX, y: inputY }],
                    borderColor: 'rgba(0,123,255,1)',
                    backgroundColor: 'rgba(0,123,255,1)',
                    borderWidth: 1,
                    pointRadius: 5,
                    fill: false,
                    showLine: false
                }
            ]
        },
        options: {
            animation: {
                duration: 0 // deshabilita la animación
            },
            scales: {
                x: {
                    type: 'linear',
                    position: 'bottom',
                    beginAtZero: true
                },
                y: {
                    type: 'linear',
                    beginAtZero: true
                }
            },
            plugins: {
                annotation: {
                    annotations: {
                        line1: {
                            type: 'line',
                            xMin: 180,
                            xMax: 180,
                            borderColor: 'rgb(255, 99, 132)',
                            borderWidth: 2
                        },
                        line2: {
                            type: 'line',
                            xMin: 360,
                            xMax: 360,
                            borderColor: 'rgb(75, 192, 192)',
                            borderWidth: 2
                        },
                        line3: {
                            type: 'line',
                            xMin: 130,
                            xMax: 130,
                            borderColor: 'rgb(153, 102, 255)',
                            borderWidth: 2
                        }
                    }
                }
            }
        }
    });
}
