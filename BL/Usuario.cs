using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Username}', '{usuario.NombreCompleto}', '{usuario.Email}',  @Password",new SqlParameter("@Password", usuario.Password));

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error occurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    int query = cnn.Database.ExecuteSqlRaw($"UsuarioUpdate '{usuario.Email}',  @Password",new SqlParameter("@Password", usuario.Password));

                    if(query > 0)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error occurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }

        public static ML.Result GetByEmail(string Email)
        {

            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    var query = cnn.Usuarios.FromSqlRaw($"UsuarioByEmail '{Email}'").ToList().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Username = query.UserName;
                        usuario.NombreCompleto = query.NombreCompleto;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;

                        result.Object = usuario;
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error occurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }

        public static ML.Result GetById(int IdUsuario)
        {

            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    var query = cnn.Usuarios.FromSqlRaw($"UsuarioByUserName {IdUsuario}").ToList().FirstOrDefault();


                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Username = query.UserName;
                        usuario.NombreCompleto = query.NombreCompleto;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;

                        result.Object = usuario;
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error occurred while inserting the record into the table" + result.Ex;
                throw;
            }
            return result;
        }

        public static ML.Result FindByEmail(string Email)
        {

            ML.Result result = new ML.Result();

            try
            {
                using (DL.MapaContext cnn = new DL.MapaContext())
                {
                    var query = cnn.Usuarios.FromSqlRaw($"UsuarioFindByEmail '{Email}'").ToList().FirstOrDefault();

                    if (query != null)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "An error occurred while inserting the record into the table" + result.Ex;
                //throw;
            }
            return result;
        }
    }
}
