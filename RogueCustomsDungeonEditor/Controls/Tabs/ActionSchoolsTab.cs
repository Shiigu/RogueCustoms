using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;

#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.


namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class ActionSchoolsTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        public event EventHandler TabInfoChanged;

        public ActionSchoolsTab()
        {
            InitializeComponent();
        }
        
        public void LoadData(DungeonInfo activeDungeon)
        {
            ActiveDungeon = activeDungeon;
            dgvSchools.Rows.Clear();
            foreach (var entry in ActiveDungeon.ActionSchoolInfos)
            {
                dgvSchools.Rows.Add(entry.Id, entry.Name);
            }
            dgvSchools.Rows[0].Selected = true;
            dgvSchools.CellValueChanged += (sender, e) => TabInfoChanged(sender, e);
        }

        public List<string> SaveData()
        {
            var validationErrors = new List<string>();
            var schoolsToSave = new List<ActionSchoolInfo>();

            dgvSchools.EndEdit();

            foreach (DataGridViewRow row in dgvSchools.Rows)
            {
                if (row.IsNewRow) continue;
                try
                {
                    var isValidSchool = true;
                    var id = row.Cells[0].Value.ToString();
                    var name = row.Cells[1].Value.ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        isValidSchool = false;
                        validationErrors.Add("At least one Action School lacks an Id.");
                    }
                    else
                    {
                        if (schoolsToSave.Any(sts => sts.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            isValidSchool = false;
                            validationErrors.Add($"Action School {id} is a duplicate.");
                        }
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            isValidSchool = false;
                            validationErrors.Add($"Action School {id} lacks a Name.");
                        }
                    }
                    if (isValidSchool)
                    {
                        schoolsToSave.Add(new()
                        {
                            Id = id,
                            Name = name
                        });
                    }
                }
                catch
                {
                    validationErrors.Add("At least one Action School is invalid.");
                }
            }

            if (!validationErrors.Any())
            {
                ActiveDungeon.ActionSchoolInfos = schoolsToSave;
            }

            return validationErrors.Distinct().ToList();
        }
    }
}
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.