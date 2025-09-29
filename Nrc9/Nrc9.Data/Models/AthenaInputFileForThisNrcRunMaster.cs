using System;
using System.Collections.Generic;

namespace Nrc9.Data.Models;

public partial class AthenaInputFileForThisNrcRunMaster
{
    public int AthenaInputFileForThisNrcRunMasterId { get; set; }

    public string PatientFirstName { get; set; } = null!;

    public string PatientLastName { get; set; } = null!;

    public string? PatientEmail { get; set; }

    public string? PatientHomePhone { get; set; }

    public string? PatientMobileNo { get; set; }

    public DateTime? ApptDate { get; set; }

    public DateTime? PatientDob { get; set; }

    public string? PatientZip { get; set; }

    public string? RndrngPrvdrFrstNme { get; set; }

    public string? RndrngPrvdrLstNme { get; set; }

    public string? RndrngPrvdrType { get; set; }

    public string? RndrngPrvdrNpiNo { get; set; }

    public string? PatientPrimaryInsHldrFi { get; set; }

    public string? PatientPrimaryInsHldrLa { get; set; }

    public string? GuarantorPhone { get; set; }

    public string? GuarantorEmail { get; set; }

    public string? SvcDeptId { get; set; }

    public string? SvcDprtmnt { get; set; }

    public string? PatientId { get; set; }

    public string? PatientPrimaryInsPkgType { get; set; }

    public string? PatientPrimaryInsPkgName { get; set; }

    public string? ProcCode { get; set; }

    public string? PatientSex { get; set; }

    public string? Race { get; set; }

    public string? Ethnicity { get; set; }

    public string? PatientLang { get; set; }

    public string? PatientAddress1 { get; set; }

    public string? PatientCity { get; set; }

    public string? PatientState { get; set; }

    public string? CurrDeptBillName { get; set; }

    public string? CurrDeptNpiNo { get; set; }

    public string? ApptId { get; set; }

    public DateTime? ApptCheckOutDate { get; set; }

    public string? PtntDcsdYsn { get; set; }

    public string? PatientMaritalStatus { get; set; }

    public string FullFileName { get; set; } = null!;

    public DateTime ImportTimestamp { get; set; }
}
