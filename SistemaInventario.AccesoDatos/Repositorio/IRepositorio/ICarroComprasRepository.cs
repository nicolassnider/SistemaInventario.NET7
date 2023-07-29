﻿using SistemaInventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICarroComprasRepository:IRepository<CarroCompras>
    {
        void Update(CarroCompras carroCompras);
    }
}
