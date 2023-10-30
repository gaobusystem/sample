using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvImport
{
    class Program
    {
        static void Main(string[] args)
        {

            const string CSV_PATH = @"C:\Users\kimura\Downloads\personal_infomation.csv";

            var list = ReadCsv(CSV_PATH);
            RegistAddresses(list);
        }

        private static List<Address> ReadCsv(string path)
        {
            List<Address> list = new List<Address>();
            var enc = new System.Text.UTF8Encoding(false);

            using (var reader = new System.IO.StreamReader(path, enc))
            {
                var csv = new CsvHelper.CsvReader(reader);
                bool isHeader = true;

                while(csv.Read())
                {
                    if (isHeader)
                    {
                        isHeader = false;
                        continue;
                    }

                    Address item = new Address()
                    {
                        Name = csv.GetField<string>(0),
                        Kana = csv.GetField<string>(1),
                        ZipCode = csv.GetField<string>(4).Replace("-", ""),
                        Prefecture = csv.GetField<string>(5),
                        StreetAddress = $"{csv.GetField<string>(6)}{csv.GetField<string>(7)}{csv.GetField<string>(8)}{csv.GetField<string>(9)}",
                        Telephone = csv.GetField<string>(2),
                        Mail = csv.GetField<string>(3)
                    };
                    list.Add(item);
                }
            }

            return list;
        }

        private static void RegistAddresses(List<Address> addresses)
        {
            using (var db = new AddressBookInfoEntities())
            {
                string groupName = "CSVインポート";

                var group = db.Groups.Where(item => item.Name.Equals(groupName)).FirstOrDefault();
                if (group == null)
                {
                    group = new Group()
                    {
                        Name = groupName
                    };
                }
                db.Groups.Add(group);

                foreach (var address in addresses)
                {
                    address.Group = group;
                    db.Addresses.Add(address);
                }
                db.SaveChanges();
            }
        }
    }
}