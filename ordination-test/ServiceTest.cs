namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligFastMedMinusEnSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            -1, 0, 0, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.ThrowsException<ArgumentException>(() => response);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligFastMedNulSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            0, 0, 0, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.ThrowsException<ArgumentException>(() => response);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
    
    [TestMethod]
    public void OpretDagligFastMedEnSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 0, 0, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(1, response.MorgenDosis.antal);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligSkaevMedMinusEnSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Dosis dosisAntal = new Dosis(DateTime.Now, -1);
        Dosis[] dosis = new Dosis[1];
        dosis[0] = dosisAntal;
        
        DagligSkæv response = service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
            dosis, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.ThrowsException<ArgumentException>(() => response);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligSkaevMedNulSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Dosis dosisAntal = new Dosis(DateTime.Now, 0);
        Dosis[] dosis = new Dosis[1];
        dosis[0] = dosisAntal;
        
        DagligSkæv response = service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
            dosis, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.ThrowsException<ArgumentException>(() => response);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
    
    [TestMethod]
    public void OpretDagligSkaevMedEnSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Dosis dosisAntal = new Dosis(DateTime.Now, 1);
        Dosis[] dosis = new Dosis[1];
        dosis[0] = dosisAntal;
        
        DagligSkæv response = service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
            dosis, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(1, response.doser.First().antal);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretPNMedMinusEnSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        
        PN response = service.OpretPN(patient.PatientId, lm.LaegemiddelId,
            -1, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.ThrowsException<ArgumentException>(() => response);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretPNMedNulSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        PN response = service.OpretPN(patient.PatientId, lm.LaegemiddelId,
            0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.ThrowsException<ArgumentException>(() => response);

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
    
    [TestMethod]
    public void OpretPNMedEnSomAntal()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        PN response = service.OpretPN(patient.PatientId, lm.LaegemiddelId,
            1, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(1, response.antalEnheder);
    }
    
    [TestMethod]
    public void OpretDagligFastMedSammeMåned()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now;
        DateTime slutDato = new DateTime(startDato.Year, startDato.Month, DateTime.DaysInMonth(startDato.Year, startDato.Month));
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 2, 0, 1, startDato, slutDato);

        Assert.AreEqual(startDato, response.startDen);
        Assert.AreEqual(slutDato, response.slutDen);
    }
    
    [TestMethod]
    public void OpretDagligFastMedNyMåned()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now;
        DateTime slutDato = new DateTime(startDato.Year, startDato.Month, 1).AddMonths(1);
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 2, 0, 1, startDato, slutDato);

        Assert.AreEqual(startDato, response.startDen);
        Assert.AreEqual(slutDato, response.slutDen);
    }
    
    [TestMethod]
    public void OpretDagligFastMedNytÅr()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now;
        DateTime slutDato = new DateTime(startDato.Year, startDato.Month, 1).AddYears(1);
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 2, 0, 1, startDato, slutDato);

        Assert.AreEqual(startDato, response.startDen);
        Assert.AreEqual(slutDato, response.slutDen);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligFastMedStartDatoMinusEnDag()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now.AddDays(-1);
        DateTime slutDato = new DateTime(startDato.Year, startDato.Month, DateTime.DaysInMonth(startDato.Year, startDato.Month));
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 2, 0, 1, startDato, slutDato);

        Assert.ThrowsException<ArgumentException>(() => response);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligFastMedSlutDatoFørStartDato()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now;
        DateTime slutDato = startDato.AddDays(-1);
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 2, 0, 1, startDato, slutDato);

        Assert.ThrowsException<ArgumentException>(() => response);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligFastUdenPatient()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now;
        DateTime slutDato = startDato.AddDays(3);
        
        DagligFast response = service.OpretDagligFast(-1, lm.LaegemiddelId,
            1, 2, 0, 1, startDato, slutDato);

        Assert.ThrowsException<ArgumentException>(() => response);
    }
    
    [TestMethod]
    public void OpretDagligFastMedPatient()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now;
        DateTime slutDato = startDato.AddDays(3);
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 2, 0, 1, startDato, slutDato);

        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligFastUdenLaegemiddel()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now;
        DateTime slutDato = startDato.AddDays(3);
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, -1,
            1, 2, 0, 1, startDato, slutDato);

        Assert.ThrowsException<ArgumentException>(() => response);
    }
    
    [TestMethod]
    public void OpretDagligFastMedLaegemiddel()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        DateTime startDato = DateTime.Now;
        DateTime slutDato = startDato.AddDays(3);
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 2, 0, 1, startDato, slutDato);

        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    public void OpretDagligFastIndenforMaxDosis()
    {
        Patient patient = service.GetPatienter().First(x => x.navn == "Jane Jensen");
        Laegemiddel lm = service.GetLaegemidler().First(x => x.navn == "Fucidin");

        DateTime startDato = DateTime.Now;
        DateTime slutDato = startDato.AddDays(3);
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            1, 0, 0, 0, startDato, slutDato);

        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void OpretDagligFastUdenforMaxDosis()
    {
        Patient patient = service.GetPatienter().First(x => x.navn == "Jane Jensen");
        Laegemiddel lm = service.GetLaegemidler().First(x => x.navn == "Fucidin");

        DateTime startDato = DateTime.Now;
        DateTime slutDato = startDato.AddDays(3);
        
        DagligFast response = service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            3, 0, 0, 0, startDato, slutDato);

        Assert.ThrowsException<ArgumentException>(() => response);
    }
    
}