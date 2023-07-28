let datatable;

$(document).ready(function () {
	loadDataTable();
});

function loadDataTable() {
	datatable = $('#tblDatos').DataTable({
		language: {
			lengthMenu: "Mostrar _MENU_ Registros por página",
			zeroRecords: "Ningún Registro",
			info: "Mostrar page _PAGE_ de _PAGES_",
			infoEmpty: "No hay registros",
			infoFiltered: "(Filtered from _MAX_ total registros)",
			search: "buscar",
			paginate: {
				first: "primero",
				last: "último",
				next: "siguiente",
				previous: "Anterior"
			}
		},
		ajax: {
			url: '/Inventario/Inventario/GetAll',
		},
		"columns": [
			{ "data": "bodega.nombre" },
			{ "data": "producto.descripcion" },
			{
				"data": "producto.costo", "className": "text-end",
				"render": function (data) {
					var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
					return d;
				}
			},
			{ "data": "cantidad", "className": "text-end" },
		]
	});
}

function Delete(url) {
	swal({
		title: "Seguro de eliminar?",
		text: "Este registro no se podrá recuperar",
		icon: "warning",
		buttons: true,
		dangerMode: true

	}).then((borrar) => {
		if (borrar) {
			$.ajax({
				type: "POST",
				url: url,
				success: function (data) {
					if (data.success) {
						toastr.success(data.message);
						datatable.ajax.reload()
					} else {
						toast.error(data.message);
					}
				}
			});
		}
	})
}
