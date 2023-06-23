window.plotFunction = (inputX) => {
    // Crear un array para tus datos x e y
    let x = [];
    let y = [];

    for (let i = 0; i <= 10000000; i += 1000000) {
        x.push(i);
        y.push(109 * i);
    }

    // Calcular el valor y correspondiente para inputX
    let inputY = 109 * inputX;

    // Obtener el elemento canvas del DOM
    let ctx = document.getElementById('myChart').getContext('2d');

    // Crear el gráfico
    let myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: x, // eje X
            datasets: [
                {
                    label: 'y = 109*x',
                    data: y, // eje Y
                    borderColor: 'rgba(255,99,132,1)', // color de la línea
                    borderWidth: 1 // ancho de la línea
                },
                {
                    label: `Punto (${inputX}, ${inputY})`,
                    data: [{ x: inputX, y: inputY }], // eje X, Y para el punto
                    borderColor: 'rgba(0,123,255,1)', // color de la línea
                    backgroundColor: 'rgba(0,123,255,1)', // color de fondo para el punto
                    borderWidth: 1, // ancho de la línea
                    pointRadius: 5, // tamaño del punto
                    fill: false, // para no llenar el área bajo la curva
                    showLine: false // para no mostrar la línea entre los puntos (solo hay uno en este caso)
                }
            ]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}