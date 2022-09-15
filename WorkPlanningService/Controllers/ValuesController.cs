using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WorkPlanningService.Models;

namespace WorkPlanningService.Controllers
{
    public class ValuesController : ApiController
    {
        WorkPlanning wp = new WorkPlanning();
        EmployeeShiftDetail ES = new EmployeeShiftDetail();
        string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        // GET api/values
        public List<EmployeeShiftDetail>Get()
        {
                        DataTable dt = new DataTable();



            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SPGetEmployeeShiftDetail"))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        
                        sda.Fill(dt);
                        List<EmployeeShiftDetail> lstEMPShift = new List<EmployeeShiftDetail>();
                        if(dt.Rows.Count>0)
                        {
                            for(int i=0;i<dt.Rows.Count;i++)
                            {
                                EmployeeShiftDetail empshift = new EmployeeShiftDetail();
                                empshift.EmpName = dt.Rows[i]["EmpName"].ToString();
                                empshift.ShiftDate = dt.Rows[i]["ShiftDate"].ToString();
                                empshift.Shifttime = dt.Rows[i]["Shifttime"].ToString();
                                lstEMPShift.Add(empshift);
                            }
                        }
                        if(lstEMPShift.Count>0)
                        {
                            return lstEMPShift;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post([FromBody] WorkPlanning wpo)
        {
            string msg = "";
            try
            {
                if (wpo != null)
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("SPSaveEmployeeShiftTime"))
                        {
                            con.Open();
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmpId", wpo.EmployeeId);
                            cmd.Parameters.AddWithValue("@Date", wpo.DateOfWorking);
                            cmd.Parameters.AddWithValue("@ShiftId", wpo.ShiftId);
                            int status = 0;
                            status = cmd.ExecuteNonQuery();
                            if (status > 0)
                                msg = "Work planning details added successfully.";
                            else
                                msg= "Something went wrong !";
                            con.Close();

                            
                        }
                    }
                }
                return msg;
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message);
                
                    return "Error while insert record :" + ex.Message;
                
            }
            
        }

        // PUT api/values/5
        public string Put(int id, [FromBody] WorkPlanning workplanningupdate)
        {
            string msg = "";
            try
            {
                if (workplanningupdate != null)
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("SPUpdateEmployeeShiftTime"))
                        {
                            con.Open();
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmpId", id);
                            cmd.Parameters.AddWithValue("@Date", workplanningupdate.DateOfWorking);
                            cmd.Parameters.AddWithValue("@ShiftId", workplanningupdate.ShiftId);
                            int status = 0;
                            status = cmd.ExecuteNonQuery();
                            if (status > 0)
                                msg = "Work planning details updated successfully.";
                            else
                                msg = "Something went wrong !";
                            con.Close();


                        }
                    }
                }
                return msg;
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message);

                return "Error while update record :" + ex.Message;

            }

        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
