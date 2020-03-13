const express = require('express')
const WebSocket = require('ws')

class Server {
  constructor(webPort, socketPort) {
    this.clients = []

    this.app = express()
    this.app.listen(webPort)
    this.app.use(express.static('public'))
    p(`web server running at port ${webPort}\n`)

    this.ws = new WebSocket.Server({ port: socketPort })
    this.ws.on('connection', this.connectionListener.bind(this))
    p(`web socket server running at port ${socketPort}\n`)
  }

  /** Initializing the client when connecting */
  connectionListener(ws, request) {
    // socketに名前をつける
    ws.name = ws._socket.remoteAddress + ":" + `${Math.random()}`.slice(2, 14)
    this.clients.push(ws)
    p(`Join ${ws.name}`)
    this.broadcast(ws, JSON.stringify({ type: 'join', name: ws.name }))

    ws.on('message', msg => {
      this.broadcast(ws, msg)
    })

    ws.on('close', () => {
      this.clients.slice(this.clients.indexOf(ws), 1)
      this.broadcast(ws, JSON.stringify({ type: 'exit', name: ws.name }))
      p(`Exit ${ws.name}`)
    })
  }

  emit(ws, data) {
    ws.send(JSON.stringify(data))
  }

  /** Push to other clients */
  broadcast(sender, message) {
    for (let c of this.clients) {
      if (c.readyState === 1) {
        c.send(message)
      }
    }
  }

  /** close server */
  close() {
    this.server.close()
  }
}

new Server(3000, 4000);

function p(message) {
  process.stdout.write(message + '\n')
}