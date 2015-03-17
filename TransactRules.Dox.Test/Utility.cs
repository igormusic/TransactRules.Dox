using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactRules.Dox.Test
{
    public static class Utility
    {
        //public static Object CreateTestConfiguration()
        //{

        //    var T4DocumentType = new ReceivedDocumentType()
        //    {
        //        Name = "T4",
        //        Description = "Last tax year copy of personal tax return",
        //        ValidForMonths = 12
        //    };

        //    var salarySlip = new ReceivedDocumentType()
        //    {
        //        Name = "Salary Slip",
        //        Description = "Copy of recent (last 12 months) salary slip",
        //        ValidForMonths = 12
        //    };

        //    var utilityBill = new ReceivedDocumentType()
        //    {
        //        Name = "Utility bill",
        //        Description = "Recent (last 3 months) utility bill (hydro, phone etc.) showing clients residential address"
        //    };

        //    var driversLicense = new ReceivedDocumentType()
        //    {
        //        Name = "Drivers License",
        //        Description = "Drivers license with photo",
        //        HasExpiryDate = true
        //    };

        //    var passport = new ReceivedDocumentType()
        //    {
        //        Name = "Passport",
        //        Description = "Passport",
        //        HasExpiryDate = true
        //    };

        //    var prCard = new ReceivedDocumentType()
        //    {
        //        Name = "Permanent Residence Card",
        //        Description = "Permanent Residence Card",
        //        HasExpiryDate = true
        //    };


        //    var buerauConsent = new GeneratedDocumentType()
        //    {
        //        Name = "Credit Bureau Consent",
        //        Description = "Credit Bureau Consent",
        //        RequireSignature = true
        //    };

        //    var loanAgreement = new GeneratedDocumentType()
        //    {
        //        Name = "Revolvng Line of Credit Loan Agreement",
        //        Description = "Revolvng Line of Credit Loan Agreement",
        //        RequireSignature = true
        //    };

        //    var preapprovedDebit = new GeneratedDocumentType()
        //    {
        //        Name = "Pre-approved debit form",
        //        Description = "Pre-approved debit form",
        //        RequireSignature = true
        //    };

        //    var processTypes =
        //            new List<ProcessType> { 
        //                new ProcessType() { 
        //                    Name = "Create client",
        //                    ReceivedDocumentSets = new List< ReceivedDocumentSet> {
        //                        new ReceivedDocumentSet () {
        //                             Name = "Proof of idenitity",
        //                             RequiredNumberOfDocuments =1,
        //                             ReceivedDocumentTypes = new List<ReceivedDocumentTypeReference> {
        //                                new ReceivedDocumentTypeReference {
        //                                     ReceivedDocumentType = driversLicense        
        //                                },
        //                                new ReceivedDocumentTypeReference {
        //                                    ReceivedDocumentType = passport
        //                                },
        //                                new ReceivedDocumentTypeReference {
        //                                    ReceivedDocumentType = prCard
        //                                }
        //                             },
        //                          },
        //                          new ReceivedDocumentSet () {
        //                             Name = "Proof of residence",
        //                             RequiredNumberOfDocuments =1,
        //                             ReceivedDocumentTypes = new List<ReceivedDocumentTypeReference> {
        //                                new ReceivedDocumentTypeReference {
        //                                     ReceivedDocumentType = driversLicense        
        //                                },
        //                                new ReceivedDocumentTypeReference {
        //                                    ReceivedDocumentType = utilityBill
        //                                }
        //                             }
                            
        //                            }
        //                        },
        //                        GeneratedDocumentSets = new List<GeneratedDocumentSet>(){
        //                            new GeneratedDocumentSet {
        //                                 Name = "Consents",
        //                                 GeneratedDocumentTypes = new List<GeneratedDocumentTypeReference> {
        //                                    new GeneratedDocumentTypeReference() {
        //                                         GeneratedDocumentType = buerauConsent
        //                                    }
        //                                 }
        //                            }
        //                        }
        //                },
        //                new ProcessType() { 
        //                    Name = "Open loan account",
        //                    ReceivedDocumentSets = new List< ReceivedDocumentSet> {
        //                        new ReceivedDocumentSet () {
        //                                Name = "Proof of Income",
        //                                RequiredNumberOfDocuments =1,
        //                                ReceivedDocumentTypes = new List<ReceivedDocumentTypeReference> {
        //                                new ReceivedDocumentTypeReference {
        //                                    ReceivedDocumentType = T4DocumentType        
        //                                },
        //                                new ReceivedDocumentTypeReference {
        //                                    ReceivedDocumentType = salarySlip
        //                                }
        //                            },
        //                        },
        //                        new ReceivedDocumentSet () {
        //                            Name = "Proof of residence",
        //                            RequiredNumberOfDocuments =1,
        //                            ReceivedDocumentTypes = new List<ReceivedDocumentTypeReference> {
        //                                new ReceivedDocumentTypeReference {
        //                                    ReceivedDocumentType = driversLicense        
        //                                },
        //                                new ReceivedDocumentTypeReference {
        //                                    ReceivedDocumentType = utilityBill
        //                                }
        //                            }
        //                        }

        //                    },
        //                    GeneratedDocumentSets = new List<GeneratedDocumentSet>(){
        //                        new GeneratedDocumentSet {
        //                             Name = "Consents and agreements",
        //                             GeneratedDocumentTypes = new List<GeneratedDocumentTypeReference> {
        //                                new GeneratedDocumentTypeReference() {
        //                                     GeneratedDocumentType = buerauConsent
        //                                },
        //                                new GeneratedDocumentTypeReference() {
        //                                     GeneratedDocumentType = loanAgreement
        //                                }

        //                             }
        //                        }
        //                    }
        //                }
        //    };

        //    return null;

        //}
    }
}
