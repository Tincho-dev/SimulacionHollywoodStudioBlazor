

setTimeout(function () {
    // Tu c�digo JavaScript aqu�
    // Se ejecutar� despu�s de 2 segundos
    document.addEventListener('DOMContentLoaded', function () {
        // Tu c�digo JavaScript aqu�
        //var botonSimular = document.getElementById('botondeSimular');
        const botonSimular = document.querySelector('.botondeSimular');
        console.log(botonSimular)
        //var contenedorRespuesta = document.getElementById('contenedorRespuesta');
        const contenedorRespuesta = document.querySelector('.contenedorRespuesta');

        //botonSimular.addEventListener('click', function () {
        //    if (contenedorRespuesta.style.display === 'none') {
        //        console.log("se muestra el contenedor")
        //        contenedorRespuesta.style.display = 'block'; // Muestra el div
        //    } else {
        //        console.log("No se muestra el contenedor")
        //        contenedorRespuesta.style.display = 'none'; // Oculta el div
        //    }
        //});
    });
}, 2000); // El valor 2000 representa los milisegundos (2 segundos)



