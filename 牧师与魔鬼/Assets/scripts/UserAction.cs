using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UserAction {
    public void goButtonIsClicked();
    public void characterIsClicked(Character characterCtrl);
    public void restart();
}

