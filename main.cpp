#include <iostream>
#include <stdlib.h>
#include <vector>
#include <sstream>
#include "miniwin.h"
using namespace std;
using namespace miniwin;
const int xi = 25, xf = 225, yi = 25, yf = 225;
const int TAMX = 250, TAMY = 290;

vector< vector<int> > desplazamiento(const vector< vector<int> > &M){
    vector< vector<int> > A;
    vector<int>::const_iterator it;
    for (int i = 0; i < M.size(); i++){
        vector<int> z(4, 0);
        const vector<int>& v = M[i];
        int j = 0;
        for (it = v.begin(); it != v.end(); it++){
            if (*it != 0) {
                z[j] = *it;
                j++;
            }
        }
        A.push_back(z);
    }
    return A;
}

vector< vector<int> > suma(const vector< vector<int> > &M){
    vector<int> z(4,0), v;
    vector< vector<int> > A;
    vector<int>::iterator it, it2;
    for (int i = 0; i < 4; i++){
        v = M[i];
        it = v.begin();
        it2 = it;
        it++;
        for(int j = 0; it2 != v.end(); it++, j++){
            if (it != v.end() && *it == *it2){
                z[j] = *it + *it2;
                it++;
            } else {
                z[j] = *it2;
            }
            it2 = it;
        }
        A.push_back(z);
        v.clear();
        z = {0, 0, 0, 0};
    }
    return A;
}

void quadricula (){
    color(NEGRO);
    rectangulo(xi, yi+30, xf, yf);
    //HORITZONTALS
    linea(xi, yi + 50, xf, yf - 150);
    linea(xi, yi + 100, xf, yf - 100);
    linea(xi, yi + 150, xf, yf - 50);
    //VERTICALS
    linea(xi + 50, yi, xf - 150, yf);
    linea(xi + 100, yi, xf - 100, yf);
    linea(xi + 150, yi, xf - 50, yf);
}

void numero(int x, int y, int n){
    int A = xi, B = A, C = xi+49, D = C;
    ostringstream a;
    a << n;

    string numero = a.str();
    if(n != 0){
        switch(n){
            case 2: color_rgb(255,251,180); break;
            case 4: color_rgb(250,250,100); break;
            case 8: color_rgb(255,255,0); break;
            case 16: color_rgb(255,160,50); break;
            case 32: color_rgb(242,121,0); break;
            case 64: color_rgb(255,85,85); break;
            case 128: color(ROJO); break;
            case 256: color_rgb(255,60,60); break;
            case 512: color(ROJO); break;
            case 1024: color_rgb(150,150,15); break;
        }
        rectangulo_lleno(A+x, B+y, C+x, D+y);
        color(NEGRO);
        texto(A + x + 20, B + y + 18, numero);
    } else{
        color_rgb(60,60,60);
        rectangulo_lleno(A+x, B+y, C+x, D+y);

    }
}

void pinta_matriu(const vector< vector<int> > &M){
    for(int i = 0; i < M.size(); i++){
        for (int j = 0; j < M[i].size(); j++){
            if(M[i][j] != 0){
                    if (i == 0){
                        if (j == 0){
                            numero(0,0, M[i][j]);
                        }else{
                            numero(0, j*50,M[i][j]);
                        }
                    }else{
                        numero(i*50, j*50,M[i][j]);
                    }
            }else{
                if (i == 0){
                        if (j == 0){
                            numero(0,0, M[i][j]);
                        }else{
                            numero(0, j*50,M[i][j]);
                        }
                    }else{
                        numero(i*50, j*50,M[i][j]);
                    }
            }
        }
    }
}

vector< vector<int> > rota(const vector< vector<int> > &M, const int i){
    vector< vector<int> > A = M;

    switch(i){
    case 1://DRETA
        for (int i = M.size()-1, a = 0; i >= 0; i--, a++){
            for(int j = M[i].size()-1, b = 0; j >= 0; j--, b++){
                A[a][b] = M[j][i];
            }
        }
        break;

    case 2://AVALL
        for(int i = M.size()-1, a = 0; i >= 0; i--, a++){
            for (int j = M[i].size()-1, b = 0; j >= 0; j--, b++){
                A[a][b] = M[i][j];
            }
        }
        break;
    case 3://ESQUERRA
        for (int i = M.size()-1, a = M.size()-1; i >= 0; i--, a--){
            for(int j = M[i].size()-1, b = M[i].size()-1; j >= 0; j--, b--){
                A[a][b] = M[j][i];
            }
        }
        break;
    return A;
    }
}
//NUMERO AL ATZAR (EN TANT PER CENT, 25% de possibilitats de que surti 4, 75% que surti 2)
int numero_A(){
    int x = rand()%100, n;
    if (x > 25){
        n = 2;

    }else{
        n = 4;
    }
    return n;
}
//NUMEROS ATZAR DESPRES DE DESPLAÇAMENT
int numero_0a3(){
    return rand()%4;
}

vector< vector<int> > matriu_numero_azar(const vector< vector<int> > &M){
    vector< vector<int> > A = M;
    int i = numero_0a3(), j = numero_0a3();
    while(M[i][j] != 0){
        i = numero_0a3(), j = numero_0a3();
    }
    A[i][j] = numero_A();
    return A;
}
//FUNCIONS AUXILIARS
vector< vector<int> > Arriba(const vector< vector<int> > &M){
    vector< vector<int> > A = M;
    A = desplazamiento(A);
    A = suma(A);
    A = matriu_numero_azar(A);
    return A;
}
vector< vector<int> > Abajo(const vector< vector<int> > &M){
    vector< vector<int> > A = M;
    A = rota(A,2);
    A = desplazamiento(A);
    A = suma(A);
    A = rota(A,2);
    A = matriu_numero_azar(A);


    return A;
}
vector< vector<int> > Derecha(const vector< vector<int> > &M){
    vector< vector<int> > A = M;
    A = rota(A,1);
    A = desplazamiento(A);
    A = suma(A);
    A = rota(A,1);
    A = matriu_numero_azar(A);
    return A;
}
vector< vector<int> > Izquierda(const vector< vector<int> > &M){
    vector< vector<int> > A = M;
    A= rota(A,3);
    A = desplazamiento(A);
    A = suma(A);
    A = rota(A,3);
    A = matriu_numero_azar(A);
    return A;
}

int main() {
    vredimensiona(TAMX, TAMY);
    vector< vector<int> > M = {
        {0, 0, 0, 0},
        {0, 0, 0, 0},
        {0, 2, 2, 0},
        {0, 0, 0, 0}
    };
    int t = tecla();
    while(t != ESCAPE){
        // Processar tecles
        switch(t){
        case ARRIBA:    M = Arriba(M);    break;
        case ABAJO:     M = Abajo(M);     break;
        case DERECHA:   M = Derecha(M);   break;
        case IZQUIERDA: M = Izquierda(M); break;
        }
        // repintar
        borra();
        quadricula();
        pinta_matriu(M);
        refresca();
        espera(50); // Mooolt important

        t = tecla();
    }
    vcierra();
    return 0;
}
