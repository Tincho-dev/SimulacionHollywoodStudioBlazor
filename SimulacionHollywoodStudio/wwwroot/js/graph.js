window.plotFunction = () => {
    // Crear un array para tus datos x e y
    let x = [];
    let y = [];

    for (let i = 0; i <= 10000000; i+=1000000) {
        x.push(i);
        y.push(161 * i);
    }

    // Obtener el elemento canvas del DOM
    let ctx = document.getElementById('myChart').getContext('2d');

    // Crear el gráfico
    let myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: x, // eje X
            datasets: [{
                label: 'y = 161*x',
                data: y, // eje Y
                borderColor: 'rgba(255,99,132,1)', // color de la línea
                borderWidth: 1 // ancho de la línea
            }]
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
