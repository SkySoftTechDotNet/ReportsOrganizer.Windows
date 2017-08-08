﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsOrganizer.DAL
{    

public interface IRepository<T> where T : IEntity
    {

        IEnumerable<T> List { get; }
        void Add(string report);
        //void Delete(T entity);
        //void Update(T entity);
        //T FindById(int Id);

    }
}
