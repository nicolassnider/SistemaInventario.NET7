let datatable;

$(document).ready(function () {
	loadDataTable();
});

function loadDataTable() {
	datatable = $('#tblDatos').DataTable({
		ajax: {
			url: '/Admin/Bodega/GetAll',
		},
		columns: [
			{ data: 'nombre', width: '20%' },
			{ data: 'descripcion', width: '40%' },
			{
				data: 'estado',
				render: function (data) {
					if (data) {
						return 'Activo';
					} else {
						return 'Inactivo';
					}
				},
				width: '40%',
			},
			{
				data: 'id',
				render: function (data) {
					return `
						<div class="text-center">
							<a href="/Admin/Bodega/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
								<i class="bi bi-pencil-square"></i>
							</a>
							<a onclick=Delete("/Admin/Bodega/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
								<i class="bi bi-trash"></i>
							</a>
						</div>
					`;
				},
				width: '20%',
			},
		],
	});
}

function Delete(url) {
	swal({
		title: "Seguro de eliminar?",
		text:"Este registro no se podrá recuperar",
		icon: "warning",
		buttons: true,
		dangerMode:true

	}).then((borrar) => {
		if (borrar) {
			$.ajax({
				type: "POST",
				url: url,
				success: function () {
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
