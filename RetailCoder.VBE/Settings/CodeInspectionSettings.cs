﻿using System;
using System.Xml.Serialization;
using Rubberduck.Inspections;
using Rubberduck.UI;

namespace Rubberduck.Settings
{
    [XmlType(AnonymousType = true)]
    public class CodeInspectionSettings
    {
        [XmlArrayItem("CodeInspection", IsNullable = false)]
        public CodeInspectionSetting[] CodeInspections { get; set; }

        public CodeInspectionSettings()
        {
            //default constructor requied for serialization
        }

        public CodeInspectionSettings(CodeInspectionSetting[] inspections)
        {
            CodeInspections = inspections;
        }
    }

    [XmlType(AnonymousType = true)]
    public class CodeInspectionSetting : IInspectionModel
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public string Description { get; set; } // not serialized because culture-dependent

        [XmlIgnore]
        public string AnnotationName { get; set; }

        [XmlIgnore]
        public CodeInspectionSeverity DefaultSeverity { get; private set; }

        [XmlAttribute]
        public CodeInspectionSeverity Severity { get; set; }

        [XmlIgnore]
        public string Meta {
            get
            {
                return InspectionsUI.ResourceManager.GetString(Name + "Meta");
            }
        }

        [XmlIgnore]
        public string TypeLabel
        {
            get { return RubberduckUI.ResourceManager.GetString("CodeInspectionSettings_" + InspectionType); }
        }

        [XmlIgnore]
        public string SeverityLabel
        {
            get { return RubberduckUI.ResourceManager.GetString("CodeInspectionSeverity_" + Severity, RubberduckUI.Culture); }
            set
            {
                foreach (var severity in Enum.GetValues(typeof (CodeInspectionSeverity)))
                {
                    if (value == RubberduckUI.ResourceManager.GetString("CodeInspectionSeverity_" + severity, RubberduckUI.Culture))
                    {
                        Severity = (CodeInspectionSeverity)severity;
                        return;
                    }
                }
            }
        }

        [XmlAttribute]
        public CodeInspectionType InspectionType { get; set; }

        public CodeInspectionSetting()
        {
            //default constructor required for serialization
        }

        public CodeInspectionSetting(string name, string description, CodeInspectionType type, CodeInspectionSeverity defaultSeverity, CodeInspectionSeverity severity)
        {
            Name = name;
            Description = description;
            InspectionType = type;
            Severity = severity;
            DefaultSeverity = defaultSeverity;
        }

        public CodeInspectionSetting(IInspectionModel inspection)
            : this(inspection.Name, inspection.Description, inspection.InspectionType, inspection.DefaultSeverity, inspection.Severity)
        { }
    }
}
