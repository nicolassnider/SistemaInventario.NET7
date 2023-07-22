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
				previuous: "Anterior"
			}
		},
		ajax: {
			url: '/Admin/Producto/GetAll',
		},
		columns: [
			{ data: 'numeroSerie' },
			{ data: 'descripcion'},
			{ data: 'categoria.nombre' },
			{ data: 'marca.nombre' },
			{
				data: 'precio',
				className: "text-end",
				render: function (data) {
					var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
					return d
				}
			},
			{
				data: 'estado',
				render: function (data) {
					if (data) {
						return 'Activo';
					} else {
						return 'Inactivo';
					}
				},				
			},
			{
				data: 'id',
				render: function (data) {
					return `
						<div class="text-center">
							<a href="/Admin/Producto/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
								<i class="bi bi-pencil-square"></i>
							</a>
							<a onclick=Delete("/Admin/Producto/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
								<i class="bi bi-trash"></i>
							</a>
						</div>
					`;
				},
			},
		],
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
