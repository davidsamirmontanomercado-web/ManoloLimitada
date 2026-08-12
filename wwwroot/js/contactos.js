document.addEventListener("DOMContentLoaded", function () {

    // MODAL EDITAR

    const editModal = document.getElementById("editModal");

    if (editModal) {

        editModal.addEventListener("show.bs.modal", function (event) {

            const button = event.relatedTarget;

            if (!button) {
                return;
            }

            // Obtener datos del botón
            const id = button.getAttribute("data-id");
            const cedula = button.getAttribute("data-cedula");
            const nombre = button.getAttribute("data-nombre");
            const apellidos = button.getAttribute("data-apellidos");
            const fecha = button.getAttribute("data-fecha");
            const telefono = button.getAttribute("data-telefono");
            const direccion = button.getAttribute("data-direccion");

            // Buscar los campos del formulario
            const inputId = editModal.querySelector("#Id");
            const inputCedula = editModal.querySelector("#Cedula");
            const inputNombre = editModal.querySelector("#Nombre");
            const inputApellidos = editModal.querySelector("#Apellidos");
            const inputFecha = editModal.querySelector("#FechaNacimiento");
            const inputTelefono = editModal.querySelector("#Telefono");
            const inputDireccion = editModal.querySelector("#Direccion");

            // Colocar los datos
            if (inputId) {
                inputId.value = id;
            }

            if (inputCedula) {
                inputCedula.value = cedula;
            }

            if (inputNombre) {
                inputNombre.value = nombre;
            }

            if (inputApellidos) {
                inputApellidos.value = apellidos;
            }

            if (inputFecha) {
                inputFecha.value = fecha;
            }

            if (inputTelefono) {
                inputTelefono.value = telefono;
            }

            if (inputDireccion) {
                inputDireccion.value = direccion;
            }
        });
    }

    // MODAL ELIMINAR

    const deleteModal = document.getElementById("deleteModal");

    if (deleteModal) {

        deleteModal.addEventListener("show.bs.modal", function (event) {

            const button = event.relatedTarget;

            if (!button) {
                return;
            }

            const id = button.getAttribute("data-id");
            const nombre = button.getAttribute("data-nombre");
            const apellidos = button.getAttribute("data-apellidos");
            const cedula = button.getAttribute("data-cedula");

            const inputId = deleteModal.querySelector("#Id");

            if (inputId) {
                inputId.value = id;
            }

            const nombreContacto =
                deleteModal.querySelector("#nombreContacto");

            const cedulaContacto =
                deleteModal.querySelector("#cedulaContacto");

            if (nombreContacto) {
                nombreContacto.textContent =
                    `${nombre} ${apellidos}`;
            }

            if (cedulaContacto) {
                cedulaContacto.textContent =
                    `Cédula: ${cedula}`;
            }
        });
    }

});