function highlightButton(button) {
    var buttons = document.querySelectorAll(".btn-group button");
    buttons.forEach(function (btn) {
        btn.classList.remove("highlighted");
    });
    button.classList.add("highlighted");
}