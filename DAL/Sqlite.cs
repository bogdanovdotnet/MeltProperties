using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Core.Entities;

using NServiceKit.DataAnnotations;
using NServiceKit.OrmLite;

namespace DAL
{
    public class Sqlite
    {
        private static OrmLiteConnectionFactory dbFactory;

        static Sqlite()
        {
            dbFactory = new OrmLiteConnectionFactory(AppDomain.CurrentDomain.BaseDirectory + "Chemical.sqlite", false, SqliteDialect.Provider);
        }

        public static void Start()
        {
            //Setup SQL Server Connection Factory
            //var dbFactory =
            //    new OrmLiteConnectionFactory(
            //        @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\App_Data\Database1.mdf;Integrated Security=True;User Instance=True", SqliteDialect.Provider);

            ////Use in-memory Sqlite DB instead
            //dbFactory = new OrmLiteConnectionFactory(AppDomain.CurrentDomain.BaseDirectory + "Chemical.sqlite", false, SqliteDialect.Provider);

            //Non-intrusive: All extension methods hang off System.Data.* interfaces
            IDbConnection db = dbFactory.OpenDbConnection();

            //Re-Create all table schemas:
            db.DropTable<Oxide>();
            db.DropTable<Phase>();
            db.DropTable<PhasesSystem>();

            db.CreateTable<Oxide>();
            db.CreateTable<Phase>();
            db.CreateTable<PhasesSystem>();
            #region data
            var sio2 = new Oxide() { Formula = "SiO2" , IsDefault = true, IsRequred = true};
            var al2o3 = new Oxide() { Formula = "Al2O3", IsDefault = true, IsRequred = true };
            var cao = new Oxide() { Formula = "CaO", IsDefault = true, IsRequred = false };
            var k2o = new Oxide() { Formula = "K20", IsDefault = true, IsRequred = false };
            var na2o = new Oxide() { Formula = "Na2O", IsDefault = true, IsRequred = false };
            var mgo = new Oxide() { Formula = "MgO", IsDefault = true, IsRequred = false };
            var tio2 = new Oxide() { Formula = "TiO2", IsDefault = false, IsRequred = false };
            var fe2o3 = new Oxide() { Formula = "Fe2O3", IsDefault = true, IsRequred = false };
            db.Save(sio2, al2o3, cao, k2o, na2o, mgo, tio2, fe2o3);
            var mno = new Oxide() { Formula = "MnO", IsDefault = false, IsRequred = false };
            var feo = new Oxide() { Formula = "FeO", IsDefault = false, IsRequred = false };
            var p2o5 = new Oxide() { Formula = "P2O5", IsDefault = false, IsRequred = false };
            var b2o3 = new Oxide() { Formula = "B2O3", IsDefault = false, IsRequred = false };
            var li2o = new Oxide() { Formula = "Li2O", IsDefault = false, IsRequred = false };
            var sro = new Oxide() { Formula = "SrO", IsDefault = false, IsRequred = false };
            var bao = new Oxide() { Formula = "BaO", IsDefault = false, IsRequred = false };
            var zno = new Oxide() { Formula = "ZnO", IsDefault = false, IsRequred = false };
            var pbo = new Oxide() { Formula = "PbO", IsDefault = false, IsRequred = false };
            var beo = new Oxide() { Formula = "BeO", IsDefault = false, IsRequred = false };
            var cuo = new Oxide() { Formula = "CuO", IsDefault = false, IsRequred = false };
            var zro2 = new Oxide() { Formula = "ZrO2", IsDefault = false, IsRequred = false };
            var sno2 = new Oxide() { Formula = "SnO2", IsDefault = false, IsRequred = false };
            var nio = new Oxide() { Formula = "NiO", IsDefault = false, IsRequred = false };
            var coo = new Oxide() { Formula = "CoO", IsDefault = false, IsRequred = false };
            var cdo = new Oxide() { Formula = "CdO", IsDefault = false, IsRequred = false };
            var f = new Oxide() { Formula = "F", IsDefault = false, IsRequred = false };
            db.Save(mno, feo, p2o5, b2o3, li2o, sro, bao, zno, pbo, beo, cuo, zro2, sno2, nio, coo, cdo, f);
            var dbOxides = db.Select<Oxide>();
            sio2 = dbOxides.FirstOrDefault(x => x.Formula.Equals(sio2.Formula));
            al2o3 = dbOxides.FirstOrDefault(x => x.Formula.Equals(al2o3.Formula));
            cao = dbOxides.FirstOrDefault(x => x.Formula.Equals(cao.Formula));
            k2o = dbOxides.FirstOrDefault(x => x.Formula.Equals(k2o.Formula));
            na2o = dbOxides.FirstOrDefault(x => x.Formula.Equals(na2o.Formula));
            mgo = dbOxides.FirstOrDefault(x => x.Formula.Equals(mgo.Formula));
            var a = new Phase() { Formula = "A" };
            a.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 100 });

            var s = new Phase() { Formula = "S" };
            s.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 100 });

            var nas2 = new Phase() { Formula = "NAS2" };
            nas2.Oxides.Add(new PhaseOxideContent() { Oxide = na2o, Percentage = 21.82f });
            nas2.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 35.88f });
            nas2.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 42.30f });

            var nas6 = new Phase() { Formula = "NAS6" };
            nas6.Oxides.Add(new PhaseOxideContent() { Oxide = na2o, Percentage = 11.82f });
            nas6.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 19.44f });
            nas6.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 68.74f });

            var n2as2 = new Phase() { Formula = "N2AS2" };
            n2as2.Oxides.Add(new PhaseOxideContent() { Oxide = na2o, Percentage = 35.82f });
            n2as2.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 29.45f });
            n2as2.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 34.72f });

            var kas6 = new Phase() { Formula = "KAS6" };
            kas6.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 18.32f });
            kas6.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 64.76f });
            kas6.Oxides.Add(new PhaseOxideContent() { Oxide = k2o, Percentage = 16.92f });

            var a3s2 = new Phase() { Formula = "A3S2" };
            a3s2.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 71.79f });
            a3s2.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 28.21f });

            var kas4 = new Phase() { Formula = "KAS4" };
            kas4.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 23.36f });
            kas4.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 55.06f });
            kas4.Oxides.Add(new PhaseOxideContent() { Oxide = k2o, Percentage = 21.58f });

            var cas2 = new Phase() { Formula = "CAS2" };
            cas2.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 36.65f });
            cas2.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 43.19f });
            cas2.Oxides.Add(new PhaseOxideContent() { Oxide = cao, Percentage = 20.16f });

            var c2as = new Phase() { Formula = "C2AS" };
            c2as.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 37.19f });
            c2as.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 21.91f });
            c2as.Oxides.Add(new PhaseOxideContent() { Oxide = cao, Percentage = 40.90f });

            var ca6 = new Phase() { Formula = "CA6" };
            ca6.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 91.60f });
            ca6.Oxides.Add(new PhaseOxideContent() { Oxide = cao, Percentage = 8.40f });

            var cs = new Phase() { Formula = "CS" };
            cs.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 51.72f });
            cs.Oxides.Add(new PhaseOxideContent() { Oxide = cao, Percentage = 48.28f });

            var ca2 = new Phase() { Formula = "CA2" };
            ca2.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 78.43f });
            ca2.Oxides.Add(new PhaseOxideContent() { Oxide = cao, Percentage = 21.57f });

            var m2a2s5 = new Phase() { Formula = "M2A2S5" };
            m2a2s5.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 34.86f });
            m2a2s5.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 51.36f });
            m2a2s5.Oxides.Add(new PhaseOxideContent() { Oxide = mgo, Percentage = 13.78f });

            var ms = new Phase() { Formula = "MS" };
            ms.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 59.85f });
            ms.Oxides.Add(new PhaseOxideContent() { Oxide = mgo, Percentage = 40.15f });

            var m2s = new Phase() { Formula = "M2S" };
            m2s.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 42.71f });
            m2s.Oxides.Add(new PhaseOxideContent() { Oxide = mgo, Percentage = 57.29f });

            var m4a5s2 = new Phase() { Formula = "M4A5S2" };
            m4a5s2.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 64.43f });
            m4a5s2.Oxides.Add(new PhaseOxideContent() { Oxide = sio2, Percentage = 15.19f });
            m4a5s2.Oxides.Add(new PhaseOxideContent() { Oxide = mgo, Percentage = 20.38f });

            var ma = new Phase() { Formula = "MA" };
            ma.Oxides.Add(new PhaseOxideContent() { Oxide = al2o3, Percentage = 71.67f });
            ma.Oxides.Add(new PhaseOxideContent() { Oxide = mgo, Percentage = 28.33f });

            db.Save(nas2, nas6, n2as2, kas6, a3s2, kas4, cas2, c2as, ca6, cs, ca2, m2a2s5, ms, m2s, m4a5s2, ma);

            var k2oSystem = new PhasesSystem() { Formula = "K2O–Al2O3–SiO2" };
            k2oSystem.Oxides.AddRange(new[] { k2o, al2o3, sio2 });
            k2oSystem.Phases.AddRange(new[] { a, s, kas4, kas6, a3s2 });
            var na2oSystem = new PhasesSystem() { Formula = "Na2O–Al2O3–SiO2" };
            na2oSystem.Oxides.AddRange(new[] { na2o, al2o3, sio2 });
            na2oSystem.Phases.AddRange(new[] { a, s, a3s2, n2as2, nas2, nas6 });
            var caoSystem = new PhasesSystem() { Formula = "CaO–Al2O3–SiO2" };
            caoSystem.Oxides.AddRange(new[] { cao, al2o3, sio2 });
            caoSystem.Phases.AddRange(new[] { a, s, a3s2, c2as, ca2, ca6, cas2, cs });
            var mgoSystem = new PhasesSystem() { Formula = "MgO–Al2O3–SiO2" };
            mgoSystem.Oxides.AddRange(new[] { mgo, al2o3, sio2 });
            mgoSystem.Phases.AddRange(new[] { a, s, a3s2, m2a2s5, m2s, m4a5s2, ma, ms });

            db.Save(k2oSystem, na2oSystem, caoSystem, mgoSystem);
            #endregion    
        }

        private static bool IsFirst = true;

        public static IEnumerable<Oxide> GetAllOxides()
        {
           IDbConnection db = dbFactory.OpenDbConnection();
            var oxides =  db.Select<Oxide>();
            return oxides;
        }

        public static IEnumerable<PhasesSystem> GetSystems(Oxide firstOxide, Oxide secondOxide)
        {
            IDbConnection db = dbFactory.OpenDbConnection();
            var phases =
                db.Select<PhasesSystem>();
            var phasesSystems = phases.Where(x => x.Oxides.Any(z => z.Formula.Equals(firstOxide.Formula)) 
                || x.Oxides.Any(z => z.Formula.Equals(secondOxide.Formula)));
            return phasesSystems;
        }
    }
}
