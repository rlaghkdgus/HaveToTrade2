using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(DialogSystem))]
public class ListSeleCtor : Editor // ����Ʈ ������ ���� ��ƿ ���
{
    public override void OnInspectorGUI()
    {
        DialogSystem selector = (DialogSystem)target;

        if (selector.DialogDB != null)
        {
            // ScriptableObject�� ����Ʈ �ʵ� �̸� ��������
            var fields = selector.DialogDB.GetType()
                .GetFields()
                .Where(f => f.FieldType == typeof(List<TextData>))
                .Select(f => f.Name)
                .ToArray();

            // ��Ӵٿ����� ����
            int currentIndex = Array.IndexOf(fields, selector.seletedDialogName);
            int newIndex = EditorGUILayout.Popup("Selected List", currentIndex, fields);

            if (newIndex >= 0 && newIndex < fields.Length)
            {
                selector.seletedDialogName = fields[newIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("ListData�� �������� �ʾҽ��ϴ�!");
        }

        if (GUILayout.Button("Dialog Load"))
        {
            selector.DialogListLoading();
        }

        // �⺻ Inspector �׸���
        DrawDefaultInspector();
    }
}
