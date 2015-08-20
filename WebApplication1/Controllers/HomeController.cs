using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using MongoDB.Driver;
using MongoRepository;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var customers = new MongoRepository<Customer>().Collection.FindAll().ToList();
            return View(customers);
        }

        public ActionResult Populate()
        {
            var repo = new MongoRepository<Customer>();
            repo.Collection.Drop();
            string file = Server.MapPath("~/Controllers/interface1.xml");
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(g));
            var wfile = new System.IO.StreamReader(file);
            var o = (g)writer.Deserialize(wfile);

            string file2 = Server.MapPath("~/Controllers/interface2.xml");
            var writer2 = new System.Xml.Serialization.XmlSerializer(typeof(root));
            var wfile2 = new System.IO.StreamReader(file2);
            var o2 = (root)writer2.Deserialize(wfile2);
            var kommuner = o2.län.SelectMany(x => x.kommun).ToList();


            foreach (var customer in o.path)
            {
                string name = "";

                var kommun = kommuner.FirstOrDefault(x => x.kommunkod == customer.id);
                if (kommun != null)
                {
                    name = kommun.kommunnamn;
                }
                var c = new Customer()
                {
                    InternalId = customer.id,
                    IsCustomer = false,
                    Path = customer.d,
                    Name = name
                };
                repo.Add(c);
            }
            return Content("!");
        }

        [Route("customer/{id}/{isCustomer}")]
        public ActionResult SetCustomer(int id, bool isCustomer)
        {
            var repo = new MongoRepository<Customer>();
            var customer =  repo.SingleOrDefault(x => x.InternalId == id);

            if (customer != null)
            {
                customer.IsCustomer = isCustomer;
                repo.Collection.Save(customer);
            }

            return Json(customer, JsonRequestBehavior.AllowGet);
        }
    }

    public class Customer : Entity
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int InternalId { get; set; }
        public bool IsCustomer { get; set; }

    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class root
    {

        private rootLän[] länField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("län")]
        public rootLän[] län
        {
            get
            {
                return this.länField;
            }
            set
            {
                this.länField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class rootLän
    {

        private byte länskodField;

        private string länsnamnField;

        private rootLänKommun[] kommunField;

        /// <remarks/>
        public byte länskod
        {
            get
            {
                return this.länskodField;
            }
            set
            {
                this.länskodField = value;
            }
        }

        /// <remarks/>
        public string länsnamn
        {
            get
            {
                return this.länsnamnField;
            }
            set
            {
                this.länsnamnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("kommun")]
        public rootLänKommun[] kommun
        {
            get
            {
                return this.kommunField;
            }
            set
            {
                this.kommunField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class rootLänKommun
    {

        private ushort kommunkodField;

        private string kommunnamnField;

        private rootLänKommunKommunvalkrets[] kommunvalkretsField;

        /// <remarks/>
        public ushort kommunkod
        {
            get
            {
                return this.kommunkodField;
            }
            set
            {
                this.kommunkodField = value;
            }
        }

        /// <remarks/>
        public string kommunnamn
        {
            get
            {
                return this.kommunnamnField;
            }
            set
            {
                this.kommunnamnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("kommunvalkrets")]
        public rootLänKommunKommunvalkrets[] kommunvalkrets
        {
            get
            {
                return this.kommunvalkretsField;
            }
            set
            {
                this.kommunvalkretsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class rootLänKommunKommunvalkrets
    {

        private byte valkretskodField;

        private string valkretsnamnField;

        private uint röstberättigadeField;

        private byte omgång1Field;

        private string restField;

        private byte omgång2Field;

        private byte mandatField;

        /// <remarks/>
        public byte valkretskod
        {
            get
            {
                return this.valkretskodField;
            }
            set
            {
                this.valkretskodField = value;
            }
        }

        /// <remarks/>
        public string valkretsnamn
        {
            get
            {
                return this.valkretsnamnField;
            }
            set
            {
                this.valkretsnamnField = value;
            }
        }

        /// <remarks/>
        public uint röstberättigade
        {
            get
            {
                return this.röstberättigadeField;
            }
            set
            {
                this.röstberättigadeField = value;
            }
        }

        /// <remarks/>
        public byte omgång1
        {
            get
            {
                return this.omgång1Field;
            }
            set
            {
                this.omgång1Field = value;
            }
        }

        /// <remarks/>
        public string rest
        {
            get
            {
                return this.restField;
            }
            set
            {
                this.restField = value;
            }
        }

        /// <remarks/>
        public byte omgång2
        {
            get
            {
                return this.omgång2Field;
            }
            set
            {
                this.omgång2Field = value;
            }
        }

        /// <remarks/>
        public byte mandat
        {
            get
            {
                return this.mandatField;
            }
            set
            {
                this.mandatField = value;
            }
        }
    }



    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class g
    {

        private gPath[] pathField;

        private string idField;

        private string classField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("path")]
        public gPath[] path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @class
        {
            get
            {
                return this.classField;
            }
            set
            {
                this.classField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class gPath
    {

        private ushort idField;

        private string classField;

        private string dField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @class
        {
            get
            {
                return this.classField;
            }
            set
            {
                this.classField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string d
        {
            get
            {
                return this.dField;
            }
            set
            {
                this.dField = value;
            }
        }
    }


}