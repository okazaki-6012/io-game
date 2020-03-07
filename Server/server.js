const http = require('http');
const WebSocket = require('ws');

class Server {
  constructor(port) {
    this.clients = []
    this.ws = new WebSocket.Server({
      port: port
    })
    this.ws.on('connection',this.connectionListener.bind(this))
    console.log(`server running at port ${port}\n`)
  }

  /** Initializing the client when connecting */
  connectionListener(ws, request) {
    // 接続があった際に掃除する
    this.clients = this.clients.filter(c => !c.destroyed)

    ws.name = ws.remoteAddress + ":" + `${Math.random()}`.slice(2, 14)
    this.clients.push(ws)
    p(`Join ${ws.name}`)

    ws.send('welcome!!')

    ws.on('message', data => this.broadcast(data, ws))

    ws.on('close', () => {
      this.clients.slice(this.clients.indexOf(ws), 1)
      p(`Exit ${ws.name}`)
    })
  }

  /** Push to other clients */
  broadcast(message, sender) {
    for (let c of this.clients) {
      if (c !== sender && c.readyState === 1) {
        c.send(message)
      }
    }
    p(message)
  }

  /** close server */
  close() {
    this.server.close()
  }
}

function p(message) {
  process.stdout.write(message + '\n')
}

module.exports = Server
