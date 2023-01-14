using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Tres_en_Raya
{
    public partial class Tres_en_Raya : Form
    {
        public Tres_en_Raya()
        {
            InitializeComponent();
        }

        /* Al iniciar activaremos el timer que hace parpadear al texto y la imagen
         * Crearemos una lista de botones
         */
        private List<Button> botones = new List<Button>();
        private void Tres_en_Raya_Load(object sender, EventArgs e)
        {
            timer.Start();
            botones.Add(btn1);
            botones.Add(btn2);
            botones.Add(btn3);
            botones.Add(btn4);
            botones.Add(btn5);
            botones.Add(btn6);
            botones.Add(btn7);
            botones.Add(btn8);
            botones.Add(btn9);
        }

        // Por cada intervalo de tiempo se hará aparecer y desaparecer al texto y la imagen
        private void timer_Tick(object sender, EventArgs e)
        {
            if (txtPresionar.Visible)
            {
                txtPresionar.Visible = false;
                joystickImage.Visible = false;
            }
            else
            {
                txtPresionar.Visible = true;
                joystickImage.Visible = true;
            }
        }

        /* Para cada partida detenemos los timers y ocultamos los controles que no se deben usar en ese momento
         * En los mensajes se mostrará que estamos configurando la partida
         */
        private void btnNuevaPartida_Click(object sender, EventArgs e)
        {
            timer.Stop();
            temporizador.Stop();

            txtPresionar.Visible = false;
            joystickImage.Visible = false;
            btnJugar.Visible = false;
            btnNuevaPartida.Visible = false;
            opcionesGroupBox.Visible = true;
            tablaBotones.Enabled = false;

            txtMensajes.Text = "Configurando Partida";
            txtTiempo.Text = "30 segundos";

            foreach (var btn in botones)
            {
                btn.Text = string.Empty;
                btn.BackColor = Color.FromName("GradientInactiveCaption");
            }
        }

        // Creamos a los jugadores como string
        private string PC = "O";
        private string Player1 = "X";
        private string Player2 = "O";

        // Los checkbox no pueden estar seleccionados al mismo tiempo ni sin seleccionar
        private void ckAspa_OnChange(object sender, EventArgs e)
        {
            ckAspa.Checked = true;
            ckCirculo.Checked = false;
        }

        private void ckCirculo_OnChange(object sender, EventArgs e)
        {
            ckCirculo.Checked = true;
            ckAspa.Checked = false;
        }

        // Asignamos quien juega primero
        private string asignarPrimero()
        {
            if (ckAspa.Checked)
            {
                turnoPlayer1 = true;
                turnoPlayer2 = false;
                turnoPC = false;
                return rdPlayer_Player.Checked ? "Turno del Jugador 1" : "Es tu turno";
            }
            else
            {
                turnoPlayer1 = false;
                turnoPlayer2 = true;
                turnoPC = rdPlayer_PC.Checked ? true : false;
                return rdPlayer_Player.Checked ? "Turno del Jugador 2" : "Turno de la Computadora";
            }
        }

        /* Este método estará disponible tanto para el btnEmpezar y para el btnJugar
         * Ocultará todos los controles que no usemos en ese momento
         * Mostraremos que tipo de partida se está jugando
         * Asignaremos el primer jugador en jugar
         */
        private void btnEmpezar_Click(object sender, EventArgs e)
        {
            opcionesGroupBox.Visible = false;
            btnJugar.Visible = true;
            btnNuevaPartida.Visible = true;

            txtPartida.Text = rdPlayer_Player.Checked ? "Player VS Player" : "Player VS PC";
            txtMensajes.Text = asignarPrimero();
            tablaBotones.Enabled = !turnoPC;

            tiempoJugador = 30;
            tiempoPC = new Random().Next(3, 8);
            txtTiempo.Text = tiempoJugador + " segundos";
            txtTiempo.ForeColor = Color.SeaGreen;
            temporizador.Start();

            ocupados.Clear();

            foreach (var btn in botones)
            {
                btn.Text = string.Empty;
                btn.BackColor = Color.FromName("GradientInactiveCaption");
                btn.Enabled = true;
            }
        }

        // Creamos los turnos de cada jugador
        private bool turnoPC = false;
        private bool turnoPlayer1 = true;
        private bool turnoPlayer2 = false;

        // Terminamos los turnos y mostramos mensajes de acuerdo al contexto
        private void terminarTurno()
        {
            tiempoJugador = 30;
            txtTiempo.Text = tiempoJugador + " segundos";
            txtTiempo.ForeColor = Color.SeaGreen;

            if (ganador())
            {
                tablaBotones.Enabled = false;
                temporizador.Stop();
                return;
            }
            else if (ocupados.Count == 9)
            {
                txtMensajes.Text = "¡Ha sido un empate!";
                temporizador.Stop();
                return;
            }

            if (rdPlayer_Player.Checked)
            {
                if (turnoPlayer1)
                {
                    turnoPlayer1 = false;
                    turnoPlayer2 = true;
                    txtMensajes.Text = "Turno del Jugador 2";
                }
                else if (turnoPlayer2)
                {
                    turnoPlayer2 = false;
                    turnoPlayer1 = true;
                    txtMensajes.Text = "Turno del Jugador 1";
                }
            }
            else if (rdPlayer_PC.Checked)
            {
                if (turnoPlayer1)
                {
                    turnoPlayer1 = false;
                    turnoPC = true;
                    txtMensajes.Text = "Turno de la Computadora";
                    tablaBotones.Enabled = false;
                    tiempoPC = new Random().Next(3, 8);
                }
                else if (turnoPC)
                {
                    turnoPC = false;
                    turnoPlayer1 = true;
                    txtMensajes.Text = "Es tu turno";
                    tablaBotones.Enabled = true;
                    tiempoPC = 0;
                }
            }
        }

        // Creamos una lista de botones ocupados
        private List<Button> ocupados = new List<Button>();

        /* El texto del botón dependerá de que si ambos jugadores juegan o solo uno con la computadora
         * También dependerá del turno de cada uno
         * Deshabilitamos el botón
         * Lo agregamos a la lista de ocupados
         * Y terminamos el turno
         */
        private void btnClick(Button btn)
        {
            btn.Text = rdPlayer_Player.Checked ? (turnoPlayer1 ? Player1 : Player2) : Player1;
            btn.Enabled = false;
            ocupados.Add(btn);
            terminarTurno();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            btnClick(btn1);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            btnClick(btn2);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            btnClick(btn3);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            btnClick(btn4);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            btnClick(btn5);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            btnClick(btn6);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            btnClick(btn7);
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            btnClick(btn8);
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            btnClick(btn9);
        }

        // Método que permite jugar a la Computadora
        private void juegaPC()
        {
            var random = new Random().Next(0, 9);

            while (ocupados.Contains(botones[random]))
            {
                random = new Random().Next(0, 9);
            }

            botones[random].Enabled = false;
            botones[random].Text = PC;
            ocupados.Add(botones[random]);
        }

        // Si alguien gana, este método mostrará quien fue y coloreará los botones que hicieron Tres en Raya
        private bool ganador()
        {
            bool ganador = false;
            var mensaje = rdPlayer_Player.Checked ? (turnoPlayer1 ? "¡Ganó el Jugador 1!" : "¡Ganó el Jugador 2!") : (turnoPlayer1 ? "¡Ganaste!" : "¡Ganó la Computadora!");

            // Si son Aspas o Círculos
            if (btn1.Text.Equals(btn2.Text) && btn2.Text.Equals(btn3.Text) && !string.IsNullOrEmpty(btn3.Text))
            {
                txtMensajes.Text = mensaje;
                btn1.BackColor = Color.FromArgb(0, 217, 187);
                btn2.BackColor = Color.FromArgb(0, 217, 187);
                btn3.BackColor = Color.FromArgb(0, 217, 187);
                ganador = true;
            }
            else if (btn4.Text.Equals(btn5.Text) && btn5.Text.Equals(btn6.Text) && !string.IsNullOrEmpty(btn6.Text))
            {
                txtMensajes.Text = mensaje;
                btn4.BackColor = Color.FromArgb(0, 217, 187);
                btn5.BackColor = Color.FromArgb(0, 217, 187);
                btn6.BackColor = Color.FromArgb(0, 217, 187);
                ganador = true;
            }
            else if (btn7.Text.Equals(btn8.Text) && btn8.Text.Equals(btn9.Text) && !string.IsNullOrEmpty(btn9.Text))
            {
                txtMensajes.Text = mensaje;
                btn7.BackColor = Color.FromArgb(0, 217, 187);
                btn8.BackColor = Color.FromArgb(0, 217, 187);
                btn9.BackColor = Color.FromArgb(0, 217, 187);
                ganador = true;
            }
            else if (btn1.Text.Equals(btn4.Text) && btn4.Text.Equals(btn7.Text) && !string.IsNullOrEmpty(btn7.Text))
            {
                txtMensajes.Text = mensaje;
                btn1.BackColor = Color.FromArgb(0, 217, 187);
                btn4.BackColor = Color.FromArgb(0, 217, 187);
                btn7.BackColor = Color.FromArgb(0, 217, 187);
                ganador = true;
            }
            else if (btn2.Text.Equals(btn5.Text) && btn5.Text.Equals(btn8.Text) && !string.IsNullOrEmpty(btn8.Text))
            {
                txtMensajes.Text = mensaje;
                btn2.BackColor = Color.FromArgb(0, 217, 187);
                btn5.BackColor = Color.FromArgb(0, 217, 187);
                btn8.BackColor = Color.FromArgb(0, 217, 187);
                ganador = true;
            }
            else if (btn3.Text.Equals(btn6.Text) && btn6.Text.Equals(btn9.Text) && !string.IsNullOrEmpty(btn9.Text))
            {
                txtMensajes.Text = mensaje;
                btn3.BackColor = Color.FromArgb(0, 217, 187);
                btn6.BackColor = Color.FromArgb(0, 217, 187);
                btn9.BackColor = Color.FromArgb(0, 217, 187);
                ganador = true;
            }
            else if (btn1.Text.Equals(btn5.Text) && btn5.Text.Equals(btn9.Text) && !string.IsNullOrEmpty(btn9.Text))
            {
                txtMensajes.Text = mensaje;
                btn1.BackColor = Color.FromArgb(0, 217, 187);
                btn5.BackColor = Color.FromArgb(0, 217, 187);
                btn9.BackColor = Color.FromArgb(0, 217, 187);
                ganador = true;
            }
            else if (btn3.Text.Equals(btn5.Text) && btn5.Text.Equals(btn7.Text) && !string.IsNullOrEmpty(btn7.Text))
            {
                txtMensajes.Text = mensaje;
                btn3.BackColor = Color.FromArgb(0, 217, 187);
                btn5.BackColor = Color.FromArgb(0, 217, 187);
                btn7.BackColor = Color.FromArgb(0, 217, 187);
                ganador = true;
            }

            return ganador;
        }

        // Los jugadores y la Computadora tienen tiempos de juego asignados
        private int tiempoJugador = 30;
        private int tiempoPC = new Random().Next(3, 8);

        // Este método nos servirá de temporizador para controlar los turnos de quienes estén jugando
        private void temporizador_Tick(object sender, EventArgs e)
        {
            if (tiempoJugador > 0 && !ganador())
            {
                tiempoJugador--;
            }
            else
            {
                terminarTurno();
            }

            if (rdPlayer_PC.Checked && turnoPC)
            {
                if (ocupados.Count == 8)
                {
                    juegaPC();
                    terminarTurno();
                }
                else if (tiempoPC > 0)
                {
                    tiempoPC--;
                }
                else
                {
                    juegaPC();
                    terminarTurno();
                }
            }

            txtTiempo.Text = tiempoJugador + (tiempoJugador != 1 ? " segundos" : " segundo");
            txtTiempo.ForeColor = tiempoJugador > 10 ? Color.SeaGreen : Color.Red;
        }

        /* Creamos movimiento para el formulario
         * Hacemos click sostenido en el contenedor de color verde claro
         * Luego movemos el mouse y el formulario se moverá
         */
        private Point mouseLocation;
        private void Tres_en_Raya_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void Tres_en_Raya_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = MousePosition;
                mousePos.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePos;
            }
        }

        // Cerramos el juego
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Minimizamos el juego
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}