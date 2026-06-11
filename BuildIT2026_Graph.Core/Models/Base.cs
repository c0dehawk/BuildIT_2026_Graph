using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildIT2026_Graph.Core.Models
{
    public class Agent
    {
        public Guid AgentId { get; set; }
        public string Name { get; set; }  
        public string DisplayName => $"AGENT";
    }

    public class Agency
    {
        public Guid AgencyId { get; set; }
        public string Name { get; set; }
        public string DisplayName => $"AGENCY";
    }

    public class Broker
    {
        public Guid BrokerId { get; set; }
        public string Name { get; set; }
        public string DisplayName => $"BROKER";
    }

    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string DisplayName => $"PRODUCT";
    }

    public class Policy
    {
        public Guid PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string DisplayName => $"POLICY";
    }

    public class Coverage
    {
        public Guid CoverageId { get; set; }
        public string Name { get; set; }
        public string DisplayName => $"COVERAGE";
    }

    public class Rider
    {
        public Guid RiderId { get; set; }
        public string Name { get; set; }
        public string DisplayName => $"RIDER";
    }

    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string DisplayName => $"INVOICE";
    }

    public class Payment
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string DisplayName => $"PAYMENT";
    }

    public class Refund
    {
        public Guid RefundId { get; set; }
        public decimal Amount { get; set; }
        public string DisplayName => $"REFUND";
    }

    public class Claim
    {
        public Guid ClaimId { get; set; }
        public string ClaimNumber { get; set; }
        public string DisplayName => $"CLAIM";
    }

    public class Incident
    {
        public Guid IncidentId { get; set; }
        public DateTime Date { get; set; }
        public string DisplayName => $"INCIDENT";
    }

    public class Payout
    {
        public Guid PayoutId { get; set; }
        public decimal Amount { get; set; }
        public string DisplayName => $"PAYOUT";
    }

    public class License
    {
        public Guid LicenseId { get; set; }
        public string LicenseNumber { get; set; }
        public string DisplayName => $"LICENSE";
    }

    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string DisplayName => $"APPOINTMENT";
    }

    public class Document
    {
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string DisplayName => $"DOCUMENT";
    }

    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string DisplayName => $"EMPLOYEE";
    }

    public class Team
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public string DisplayName => $"TEAM";
    }

    public class Application
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public string DisplayName => $"APPLICATION";
    }
}
