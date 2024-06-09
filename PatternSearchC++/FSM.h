
#ifndef FSM_H_INCLUDED
#define FSM_H_INCLUDED
#define total 256


int Next_s(string P, int M, int state, int x) {
    if (state < M && x == P[state])
        return state + 1;
    int next, i;
    for (next = state; next > 0; next--) {
        if (P[next - 1] == x) {
            for (i = 0; i < next - 1; i++)
                if (P[i] != P[state - next + 1 + i])
                    break;
            if (i == next - 1)
                return next;
        }
    }

}

void Build(string P, int M, int TF[][total]) //The function is responsible for building the Automata
{
    int state, x;
    for (state = 0; state <= M; ++state)
        for (x = 0; x < total; ++x)
            TF[state][x] = Next_s(P, M, state, x);
}

void FSM(string T, string P) // This function is meant for printing
 {
    int LT = T.length();
    int LP = P.length();

    auto TF= new int [LP + 1][total];
    Build(P, LP, TF);
    int i, state = 0;
    for (i = 0; i < LT; i++)
    {

        state = TF[state][T[i]];
        if (state == LP)
        {
            //cout << "FSM: "<<i - LP + 1<<endl;

        }


    }

}
#endif // FSM_H_INCLUDED
