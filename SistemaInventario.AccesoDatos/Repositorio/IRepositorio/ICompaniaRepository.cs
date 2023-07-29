﻿using SistemaInventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICompaniaRepository:IRepository<Compania>
    {
        void Update(Compania compania);
    }
}
