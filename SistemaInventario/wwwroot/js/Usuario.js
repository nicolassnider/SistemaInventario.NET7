let datatable;

$(document).ready(function () {
	loadDataTable();
});

function loadDataTable() {
	datatable = $('#tblDatos').DataTable({
		languaje: {
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
				previuous:"Anterior"
			}
		},
		ajax: {
			url: '/Admin/Usuario/GetAll',
		},
		columns: [
			{ data: 'email' },
			{ data: 'nombres' },
			{ data: 'apellido' },
			{ data: 'phoneNumber' },
			{ data: 'role' },
			{
				data: {
					id: 'id',lockoutEnd:'' /*trabajar con dos columnas*/
				},
				render: function (data) {
					let hoy = new Date().getTime();
					let bloqueo = new Date(data.lockoutEnd).getTime();
					if (bloqueo > hoy) {
						//usuario bloqueado
						return `<div class="text-center">
									<a onclick=BloquearDesbloquear('${data.id}') class ="btn btn-primary text-white" style="cursor:pointer, width:80%">
										<i class="bi bi-unlock-fill">Desbloquear</i>
									</a>
								</div>`;
					} else {
						return `<div class="text-center">
									<a onclick=BloquearDesbloquear('${data.id}') class ="btn btn-danger text-white" style="cursor:pointer; width:80%">
										<i class="bi bi-lock-fill">Bloquear</i>
									</a>
								</div>`;
					}
				}
			}
		],
	});
}

function BloquearDesbloquear(id) {
	$.ajax({
		type: "POST",
		url: '/Admin/Usuario/BloquearDesbloquear',
		data: JSON.stringify(id),
		contentType:"application/json",
		success: function (data) {
			if (data.success) {
				toastr.success(data.message);
				datatable.ajax.reload();
			}
			else {
				toastr.error(data.message);
			}
		}
	})
}
