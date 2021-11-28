using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    //GameObject�i�[�ϐ��̐錾 D��dropGameObject�AL��GameObject(1)(�������C�A�E�g)
    public GameObject D, L;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);

        //�h���b�v�e�N�X�`���ݒ�p�ϐ�
        int type;
        //�����ՖʃR���{CHK�p�z��
        int[,] typeChk = new int[7, 8]
        {
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
        };
        //�Ֆʃ��[�v(�c5�s��)
        for (int i = 0; i < 5; i++)
        {
            //GameObject��L��V����GameObject�Ƃ��ĕ���
            GameObject l = Instantiate(L) as GameObject;
            //�e��ύX���A�e��transform����ɂ���
            l.transform.SetParent(transform);
            //�e�̃X�P�[������ɃX�P�[���ύX�AVector3.one��Vector3(1, 1, 1)�Ɠ��`
            //��Vector3�^��3��float�^�̒l���܂Ƃ߂ĕێ��ł���^
            l.transform.localScale = Vector3.one;
            //�Ֆʃ��[�v(��6��)
            for (int j = 0; j < 6; j++)
            {
                //GameObject��D��V����GameObject�Ƃ��ĕ���
                GameObject d = Instantiate(D) as GameObject;
                //�e��ύX���A�e��transform����ɂ���
                d.transform.SetParent(l.transform);
                while (true)
                {
                    //0�`5�܂ł̐����������_���Ő錾�����ϐ��֊i�[(�h���b�v�e�N�X�`���̌���)
                    type = Random.Range(0, 6);
                    typeChk[i + 2, j + 2] = type;
                    //��2��or��2�̃h���b�v��type�Ɠ����ꍇ�A�ʂ̐F�ɕύX
                    if (type == typeChk[i + 2, j] && type == typeChk[i + 2, j + 1]
                        || type == typeChk[i, j + 2] && type == typeChk[i + 1, j + 2]) {
                        continue;
                    }
                    break;
                }
                //��������GameObjectd�̃R���|�[�l���g(DropCnt)���擾���ASet�֐����R�[����type(�����_���l)�������n��
                d.GetComponent<DropCnt>().Set(type);
                //�h���b�v����ւ��p��ID���Z�b�g
                d.GetComponent<DropCnt>().ID1 = i;
                d.GetComponent<DropCnt>().ID2 = j;
                //GameObject[D]��T���A�R���|�[�l���g(Director)���擾���A�h���b�v(d)��Obj�ɁA�h���b�v�e�N�X�`�����(type)��Field�Ɋi�[
                GameObject.Find("D").GetComponent<Director>().Obj[i, j] = d;
                GameObject.Find("D").GetComponent<Director>().Field[i, j] = type;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
