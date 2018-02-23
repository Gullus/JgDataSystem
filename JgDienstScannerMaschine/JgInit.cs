﻿using JgLibHelper;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.IO;

namespace JgDienstScannerMaschine
{
    public class JgInit
    {
        public JgOptionen _JgOpt;

        private string _FileMaschinen { get => _JgOpt.PfadDaten + "JgMaschinen.xml"; }
        private string _FileBediener { get => _JgOpt.PfadDaten + "JgBediener.xml"; }

        public JgInit(JgOptionen MyOptionen)
        {
            _JgOpt = MyOptionen;
        }

        public bool BedienerVonServer()
        {
            var speichern = false;
            var copyBenutzer = new JgLibHelper.JgCopyProperty<ServiceRef.JgWcfBediener>();

            try
            {
                using (var dienst = new ServiceRef.WcfServiceClient())
                {
                    var lWcfBediener = dienst.GetBediener();

                    foreach (var bedWcf in lWcfBediener)
                    {
                        JgBediener bedMaschine = null;

                        if (_JgOpt.ListeBediener.ContainsKey(bedWcf.Id))
                            bedMaschine = _JgOpt.ListeBediener[bedWcf.Id];
                        else
                        {
                            bedMaschine = new JgBediener() { Id = bedWcf.Id, Aenderung = DateTime.Now };
                            _JgOpt.ListeBediener.Add(bedWcf.Id, bedMaschine);
                        }

                        if (bedMaschine.Aenderung != bedWcf.Aenderung)
                        {
                            speichern = true;
                            copyBenutzer.CopyProperties(bedWcf, bedMaschine);
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                JgLog.Set(null, $"Fehler Wcf Bediener!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }

            return speichern;
        }

        public void BedienerLocalLaden()
        {
            try
            {
                var lBediener = Helper.XmlDateiInObjekt<JgBediener[]>(_FileBediener);
                if (lBediener != null)
                {
                    _JgOpt.ListeBediener.Clear();
                    foreach (var ma in lBediener)
                        _JgOpt.ListeBediener.Add(ma.Id, ma);
                }
            }
            catch (Exception ex)
            {
                JgLog.Set(null, $"Fehler beim localen laden Bediener!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }

        public void BedienerLocalSpeichern()
        {
            var arSpeichern = new JgBediener[_JgOpt.ListeBediener.Count];
            _JgOpt.ListeBediener.Values.CopyTo(arSpeichern, 0);

            try
            {
                Helper.ObjektInXmlDatei<JgBediener[]>(arSpeichern, _FileBediener);
            }
            catch (Exception ex)
            {
                JgLog.Set(null, $"Fehler beim localen speichern der Bediener!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }

        public bool MaschinenVonServer()
        {
            var copyMaschine = new JgCopyProperty<ServiceRef.JgWcfMaschine>();
            var speichern = true;

            try
            {

                using (var dienst = new ServiceRef.WcfServiceClient())
                {
                    var lWcfMaschinen = dienst.GetMaschinen(_JgOpt.IdStandort);
                    foreach (var maWcf in lWcfMaschinen)
                    {
                        JgMaschineStamm maMaschine = null;

                        if (_JgOpt.ListeMaschinen.ContainsKey(maWcf.Id))
                        {
                            maMaschine = _JgOpt.ListeMaschinen[maWcf.Id];
                            if (maMaschine.Aenderung != maWcf.Aenderung)
                            {
                                speichern = true;
                                copyMaschine.CopyProperties(maWcf, maMaschine);
                            }

                        }
                        else
                        {
                            speichern = true;

                            switch (maWcf.MaschineArt)
                            {
                                case JgLibHelper.MaschinenArten.Hand:
                                    maMaschine = new JgMaschineHand();
                                    break;
                                case JgLibHelper.MaschinenArten.Evg:
                                    maMaschine = new JgMaschineEvg();
                                    break;
                                case JgLibHelper.MaschinenArten.Schnell:
                                    maMaschine = new JgMaschineSchnell();
                                    break;
                                case JgLibHelper.MaschinenArten.Progress:
                                    maMaschine = new JgMaschineProgress();
                                    break;
                            }

                            copyMaschine.CopyProperties(maWcf, maMaschine);
                            _JgOpt.ListeMaschinen.Add(maWcf.Id, maMaschine);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(new Exception("Fehler beim Wcf Maschine laden!", ex), "Policy");
            }

            return speichern;
        }

        public void MaschinenLocalLaden()
        {
            if (!File.Exists(_FileMaschinen))
                return;

            try
            {
                var maschinenTypes = new Type[] { typeof(JgMaschineEvg), typeof(JgMaschineSchnell), typeof(JgMaschineProgress), typeof(JgMaschineHand) };
                var lMaschinen = Helper.XmlDateiInObjekt<JgMaschineStamm[]>(_FileMaschinen, maschinenTypes);

                if (lMaschinen != null)
                {
                    _JgOpt.ListeMaschinen.Clear();
                    foreach (var ma in lMaschinen)
                        _JgOpt.ListeMaschinen.Add(ma.Id, ma);
                }
            }
            catch (Exception ex)
            {
                JgLog.Set(null, $"Fehler locales laden XML Daten Maschine!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }

        public void MaschinenLocalSpeichern()
        {
            var arSpeichern = new JgMaschineStamm[_JgOpt.ListeMaschinen.Count];
            _JgOpt.ListeMaschinen.Values.CopyTo(arSpeichern, 0);

            try
            {
                var maschinenTypes = new Type[] { typeof(JgMaschineHand), typeof(JgMaschineEvg), typeof(JgMaschineSchnell), typeof(JgMaschineProgress) };
                Helper.ObjektInXmlDatei<JgMaschineStamm[]>(arSpeichern, _FileMaschinen, maschinenTypes);
            }
            catch (Exception ex)
            {
                JgLog.Set(null, $"Fehler beim localen speichern der Maschinen!\nGrund: {ex.Message}", JgLog.LogArt.Fehler);
            }
        }

        public static JgMaschineStamm GetMaschine(Dictionary<Guid, JgMaschineStamm> ListeMaschinen, Guid IdMaschine)
        {
            if (ListeMaschinen.ContainsKey(IdMaschine))
                return ListeMaschinen[IdMaschine];

            return null;
        }
    }
}
